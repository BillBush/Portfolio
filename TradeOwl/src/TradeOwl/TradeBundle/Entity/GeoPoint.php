<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 9/15/14
 * Time: 1:10 PM
 */

namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblGeoPoint")
 * @ORM\HasLifecycleCallbacks
 */
class GeoPoint
{
    //<editor-fold desc="variables">
    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="geoPoint_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;
    /**
     * @ORM\Column(name="geoPoint_longitude", type="float", nullable=false)
     */
    protected $lng;
    /**
     * @ORM\Column(name="geoPoint_latitude", type="float", nullable=false)
     */
    protected $lat;
    /**
     * @ORM\ManyToOne(targetEntity="GeoPolygon",fetch="EXTRA_LAZY", inversedBy="points", cascade={"persist", "remove"})
     * @ORM\JoinColumn(name="geoPoly_id", referencedColumnName="geoPoly_id")
     */
    protected $polygon;
    //</editor-fold>
    public function __toStringForPolygon(){
        return sprintf('%f %f', $this->getLng(), $this->getLat());
    }
    /**
     * Get id
     *
     * @return integer
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Set lng
     *
     * @param float $lng
     * @return GeoPoint
     */
    public function setLng($lng)
    {
        $this->lng = $lng;

        return $this;
    }

    /**
     * Get lng
     *
     * @return float
     */
    public function getLng()
    {
        return $this->lng;
    }

    /**
     * Set lat
     *
     * @param float $lat
     * @return GeoPoint
     */
    public function setLat($lat)
    {
        $this->lat = $lat;

        return $this;
    }

    /**
     * Get lat
     *
     * @return float
     */
    public function getLat()
    {
        return $this->lat;
    }

    /**
     * Set polygon
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoPolygon $polygon
     * @return GeoPoint
     */
    public function setPolygon(\TradeOwl\TradeBundle\Entity\GeoPolygon $polygon = null)
    {
        $this->polygon = $polygon;

        return $this;
    }

    /**
     * Get polygon
     *
     * @return \TradeOwl\TradeBundle\Entity\GeoPolygon
     */
    public function getPolygon()
    {
        return $this->polygon;
    }
}
