<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 9/15/14
 * Time: 1:19 PM
 */

namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblGeoPolygon")
 * @ORM\HasLifecycleCallbacks
 */
class GeoPolygon
{
    //<editor-fold desc="variables">
    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="geoPoly_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;
    /**
     * @ORM\OneToMany(targetEntity="GeoPoint", mappedBy="polygon", fetch="EAGER", cascade={"persist", "remove"})
     */
    protected $points;
    /**
     * @ORM\ManyToOne(targetEntity="GeoFilter", inversedBy="polygons", cascade={"persist", "remove"})
     * @ORM\JoinColumn(name="geoFilter_id", referencedColumnName="geoFilter_id")
     */
    protected $filter;
    //</editor-fold>
    public function __toString(){
        $retStr ='POLYGON((';
        $first = true;
        foreach($this->getPoints() as $point){
            if (!$first){
                $retStr = $retStr . ", ";
            }
            $retStr = $retStr . $point->__toStringForPolygon();
            $first = false;
        }
        $retStr = $retStr . ", " . $this->getPoints()->first()->__toStrin() . "))";
        return $retStr;
    }
    /**
     * @param \Doctrine\Common\Collections\ArrayCollection $points
     */
    public function resetPoints($points = null)
    {
        if ($points === null) {
            $this->points = new \Doctrine\Common\Collections\ArrayCollection();
        } else {
            $this->points = $points;
        }
    }

    /**
     * @ORM\PreUpdate
     *
     * @return $this
     */
    public function syncPoints(){
        foreach($this->getPoints() as $point){
            $point->setPolygon($this);
        }
        return $this;
    }

    /**
     * Constructor
     */
    public function __construct()
    {
        $this->points = new \Doctrine\Common\Collections\ArrayCollection();
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
     * Add points
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoPoint $points
     * @return GeoPolygon
     */
    public function addPoint(\TradeOwl\TradeBundle\Entity\GeoPoint $points)
    {
        $this->points[] = $points;
        $points->setPolygon($this);

        return $this;
    }

    /**
     * Remove points
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoPoint $points
     */
    public function removePoint(\TradeOwl\TradeBundle\Entity\GeoPoint $points)
    {
        $this->points->removeElement($points);
    }

    /**
     * Get points
     *
     * @return \Doctrine\Common\Collections\Collection
     */
    public function getPoints()
    {
        return $this->points;
    }

    /**
     * Set user
     *
     * @param \TradeOwl\TradeBundle\Entity\User $user
     * @return GeoPolygon
     */
    public function setUser(\TradeOwl\TradeBundle\Entity\User $user = null)
    {
        $this->user = $user;

        return $this;
    }


    /**
     * Set filter
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoFilter $filter
     * @return GeoPolygon
     */
    public function setFilter(\TradeOwl\TradeBundle\Entity\GeoFilter $filter = null)
    {
        $this->filter = $filter;

        return $this;
    }

    /**
     * Get filter
     *
     * @return \TradeOwl\TradeBundle\Entity\GeoFilter
     */
    public function getFilter()
    {
        return $this->filter;
    }
}
