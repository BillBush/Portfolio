function generateHTML(response, editOrView) {
    console.log(response);
    var html = '<div>';
    var index = response.length;
    for (var i = 0; i < index; i++) {
        var classInsert = "";
        if (i > 9) {
            var classInsert = "hidden";
        }

        var i = i.toString();
        html +=

            '<div class="panel panel-default tradePost' + classInsert + '" id="' + i + '">' +
            '<div class="panel-heading">';
//        if (editOrView == 'view'){
            html += '<a href="/post_view/' + response[i]["id"];
//        } else if (editOrView == 'edit'){
//            html += '<a href="/post_edit/' + response[i]["id"];
//        }
        html += '">' + response[i]["title"] +
            '</a>' +

            '<span class="pull-right">' +
            response[i]["postDate"] +
            '</span>' +
            '</div>' +

            '<div class="panel-body tradeText"><div class="thumb left smallThumb">' +
            '<a class="thumbnail" href="';
//            if (editOrView == 'view'){
                html += '/post_view/' + response[i]["id"];
//            } else if (editOrView == 'edit'){
//                html += '/post_edit/' + response[i]["id"];
//            }
            html += '">' +
            '<img src="' + response[i]["img"] + '">' +
            '</a>' +
            '</div><p>' +
            response[i]["body"] +
            '</p></div>' +
            '</div>';
        if (editOrView == 'edit'){
            html += '<input id="postBtn_Delete_' + response[i]["id"]
            + '" type="button" class="postBtn" value="Delete"/>\n';
            html += '<input id="postBtn_Edit_' + response[i]["id"]
            + '" type="button" class="postBtn" value="Edit"/>\n';
        }
    }

    html += '</div>';
    if (response["response"]) {
        html = '<div class="alert alert-dismissable alert-danger">' +
        '<button type="button" class="close" data-dismiss="alert">×</button>' +
        '<strong>Oh snap!</strong> "No Posts Found! Please try searching for something else!"</div>';
    }

//    $("#postings").children().remove();
    $("#postings").append(html);


    var index = response.length;

    for (i=0;i<index;i++){
        if ($("#"+i+" .tradeText p").html().length > 200) {
            $("#"+i+" .tradeText p"  ).html().substring(0,200);
            $("#" + i + " .tradeText p").append('... <a href="/post_view/' + response[i]['id'] + '">[read more]</a>');
        }

        //else {$("#" + i + " .tradeText p").append('<br /><div><br /> <a href="/post_view/' + response[i]['id'] + '">[Read Trade Post]</a></div>');}
    }
    /*
    $(function () {
        $(".pager").pagination({
            items: response.length,
            itemsOnPage: 10,
            cssStyle: 'light-theme',
            onPageClick: function (pageNumber, event) {
                var currPage;
                var pagePosts;
                currPage = $(".pager").pagination('getCurrentPage');
                pagePosts = $(".panel").get();
                $(".panel").toggleClass("hidden");
            }
        });
    });
    */
}

function runSearch(e) {
    $("#postings").html(" ");
    event.preventDefault();
    event.stopImmediatePropagation();
    var searchPath = generateSearch();
    $.ajax({
        type: "GET",
        async: true,
        url: searchPath,
        dataType: "json",
        data: $('#searchForm').serialize(),
        success: function (response) {
            generateHTML(response, 'view');
        },
        error: function (xml, textStatus, errorThrown) {
            alert("This is an error: " + xml.status + "||" + xml.responseText);
        }
    });
}