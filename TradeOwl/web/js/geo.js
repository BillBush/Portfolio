var numToPolyHash = {};
var polyToNumHash = {};
var polyCnt = 0;
var map;
var newFilterId = "";
var newFilterName = "";
var polygons = [];
function reset(){
    numToPolyHash = {};
    polyToNumHash = {};
    polyCnt = 0;
    $('#geo_polygons_list').empty();
    $('#geo_filter_name').val('');
    while(polygons[0]){
        polygons.pop().setMap(null);
    }
    newFilterId = "";
    newFilterName = "";
}
function initialize() {
    polyCnt = 0;
    var mapOptions = {
        center: new google.maps.LatLng(30.72058, -88.091125),//TODO: derive this from the IP
        zoom: 8
    };

    map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    var drawingManager = new google.maps.drawing.DrawingManager({
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: [
                google.maps.drawing.OverlayType.POLYGON
            ]
        }
    });
    drawingManager.setMap(map);
    google.maps.event.addDomListener(drawingManager, 'polygoncomplete', function (polygon) {
        polyCnt++;
        var str = '<div>\n\r'
            + '    <div id="geo_polygons_' + polyCnt + '">\n\r';
        var pntCnt = 0;
        polygon.getPath().forEach(function (xy, i) {
            console.log(xy.lng() + ' ' + xy.lat())
            str += '       <div>\n\r'
            + '        <div id="geo_points_' + polyCnt + '_' + pntCnt + '">\n\r'
            + '            <div>\n\r'
            + '                <input class="hidden" type="text" id="geo_points_' + polyCnt + '_' + pntCnt + '_lng" name="geo_filter[polygons][' + polyCnt + '][points][' + pntCnt + '][lng]" value="' + xy.lng() + '">\n\r'
            + '            </div>\n\r'
            + '            <div>\n\r'
            + '                <input class="hidden" type="text" id="geo_points_' + polyCnt + '_' + pntCnt + '_lat" name="geo_filter[polygons][' + polyCnt + '][points][' + pntCnt + '][lat]" value="' + xy.lat() + '">\n\r'
            + '            </div>\n\r'
            + '        </div>\n\r'
            + '    </div>\n\r'
            pntCnt++;
        });
        str += '   </div>\n\r</div>';
        $('#geo_polygons_list').append(str);
        polygons.push(polygon);
        polyToNumHash[getPolyString(polygon)] = polyCnt;
        numToPolyHash[polyCnt] = polygon;
        google.maps.event.addListener(polygon, 'click', function () {//TODO: make this a named function
            console.log('' + polyToNumHash[getPolyString(this)]);
            console.log('' + getPolyString(this));
            $('#geo_polygons_' + polyToNumHash[getPolyString(this)]).parent().remove();
            this.setMap(null);
        });
    });
    for (polyCnt = 0; polyCnt < $('#geo_polygons_list > div').length; polyCnt++) {
        var coords = [];
        for (var pntCnt = 0; pntCnt < $('#geo_polygons_' + polyCnt + ' > div').length; pntCnt++) {
            var lng = $('#geo_points_' + polyCnt + '_' + pntCnt + '_lng').val();
            var lat = $('#geo_points_' + polyCnt + '_' + pntCnt + '_lat').val();
            coords.push(new google.maps.LatLng(lat, lng));
        }
        var newPoly = new google.maps.Polygon({
            paths: coords,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35
        });
        newPoly.setMap(map);
        polygons.push(newPoly);
        polyToNumHash[getPolyString(newPoly)] = polyCnt;
        numToPolyHash[polyCnt] = newPoly;
        google.maps.event.addListener(newPoly, 'click', function () {
            console.log('' + polyToNumHash[getPolyString(this)]);
            console.log('' + getPolyString(this));
            $('#geo_polygons_' + polyToNumHash[getPolyString(this)]).parent().remove();
            this.setMap(null);
        });
    }
    $('#myModal').on('shown.bs.modal', function () {
        google.maps.event.trigger(map, 'resize');
    });
    $('#myModal').on('hidden.bs.modal', function () {
        if (String(newFilterId) != "") {
            $('#post_geoFilter').append("<option value='" + newFilterId + "'>" + newFilterName + "</option>");
            $('#post_geoFilter').val(newFilterId);
            reset();
        }
    });
    $("#geoSubmitButton").click(function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        $.ajax({
            type: "POST",
            async: true,
            url: "/geo_save/",
            dataType: "json",
            data: $('#geoForm').serialize(),
            success: function (response) {
                newFilterId = response['id'];
                newFilterName = response['name'];
                $('#myModal').modal('toggle');
            },
            error: function (xml, textStatus, errorThrown) {
                alert("This is an error: " + xml.status + "||" + xml.responseText);
            }
        });
    });
    $('#modalButton').click(function (evenet){
        event.preventDefault();
        event.stopImmediatePropagation();
        $('#myModal').modal('toggle');
    });
}
function getPolyString(polygon) {
    var retStr = '';
    polygon.getPath().forEach(function (xy, i) {
        retStr += xy.lng() + '' + xy.lat();
    });
    return retStr;
}
google.maps.event.addDomListener(window, 'load', initialize);
//end geo
