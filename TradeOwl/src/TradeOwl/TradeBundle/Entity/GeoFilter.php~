<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 9/19/14
 * Time: 11:33 AM
 */

namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblGeoFilter")
 * @ORM\HasLifecycleCallbacks
 */
class GeoFilter
{
    //<editor-fold desc="variables">
    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="geoFilter_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;
    /**
     * @ORM\OneToMany(targetEntity="GeoPolygon", mappedBy="filter", fetch="EAGER", cascade={"persist", "remove"})
     */
    protected $polygons;
    /**
     * @ORM\ManyToOne(targetEntity="User", inversedBy="geoFilters")
     * @ORM\JoinColumn(name="user_id", referencedColumnName="user_id")
     */
    protected $user;
    /**
     * @ORM\OneToMany(targetEntity="PostItem", mappedBy="geoFilter", fetch="EXTRA_LAZY")
     */
    protected $posts;
    /**
     * @ORM\Column(name="geoFilter_name", type="string", length=80, nullable=false)
     */
    protected $name;
    //</editor-fold>
    /**
     * @param \Doctrine\Common\Collections\ArrayCollection $polygons
     */
    public function resetPolygons($polygons = null)
    {
        if ($polygons === null) {
            $this->polygons = new \Doctrine\Common\Collections\ArrayCollection();
        } else {
            $this->polygons = $polygons;
        }
    }

    /**
     * @ORM\PreUpdate
     *
     * @return $this
     */
    public function syncPolygons(){
        foreach($this->getPolygons() as $polygon){
            $polygon->setFilter($this);
        }
        return $this;
    }

    /**
     * Constructor
     */
    public function __construct()
    {
        $this->polygons = new \Doctrine\Common\Collections\ArrayCollection();
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
     * Add polygons
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoPolygon $polygons
     * @return GeoFilter
     */
    public function addPolygon(\TradeOwl\TradeBundle\Entity\GeoPolygon $polygons)
    {
        $this->polygons[] = $polygons;
        $polygons->setFilter($this);

        return $this;
    }

    /**
     * Remove polygons
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoPolygon $polygons
     */
    public function removePolygon(\TradeOwl\TradeBundle\Entity\GeoPolygon $polygons)
    {
        $this->polygons->removeElement($polygons);
    }

    /**
     * Get polygons
     *
     * @return \Doctrine\Common\Collections\Collection
     */
    public function getPolygons()
    {
        return $this->polygons;
    }

    /**
     * Set user
     *
     * @param \TradeOwl\TradeBundle\Entity\User $user
     * @return GeoFilter
     */
    public function setUser(\TradeOwl\TradeBundle\Entity\User $user = null)
    {
        $this->user = $user;

        return $this;
    }

    /**
     * Get user
     *
     * @return \TradeOwl\TradeBundle\Entity\User
     */
    public function getUser()
    {
        return $this->user;
    }

    /**
     * Set name
     *
     * @param string $name
     * @return GeoFilter
     */
    public function setName($name)
    {
        $this->name = $name;

        return $this;
    }

    /**
     * Get name
     *
     * @return string
     */
    public function getName()
    {
        return $this->name;
    }

    /**
     * Add posts
     *
     * @param \TradeOwl\TradeBundle\Entity\PostItem $posts
     * @return GeoFilter
     */
    public function addPost(\TradeOwl\TradeBundle\Entity\PostItem $posts)
    {
        $this->posts[] = $posts;

        return $this;
    }

    /**
     * Remove posts
     *
     * @param \TradeOwl\TradeBundle\Entity\PostItem $posts
     */
    public function removePost(\TradeOwl\TradeBundle\Entity\PostItem $posts)
    {
        $this->posts->removeElement($posts);
    }

    /**
     * Get posts
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getPosts()
    {
        return $this->posts;
    }
}
