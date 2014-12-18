<?php
namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use Symfony\Component\Validator\Constraints\NotBlank;
use Symfony\Component\Validator\Mapping\ClassMetadata;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblPostItem")
 * @ORM\HasLifecycleCallbacks
 */
class PostItem
{
    //<editor-fold desc="variables">
    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="postItem_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;


    /**
     * @ORM\ManyToMany(targetEntity="TradeOffer", mappedBy="posts", cascade={"persist"})
     **/
    private $offers;
    /**
     * @ORM\ManyToOne(targetEntity="User", inversedBy="posts", fetch="EXTRA_LAZY")
     * @ORM\JoinColumn(name="user_id", referencedColumnName="user_id")
     */
    protected $user;
    /**
     * @ORM\OneToMany(targetEntity="PostPicture", mappedBy="postItem", fetch="EAGER", cascade={"persist", "remove"})
     */
    protected $pics; //TODO:handle cleanup of removed post images
    /**
     * @ORM\OneToMany(targetEntity="Comment", mappedBy="postItem", fetch="EAGER")
     */
    protected $comments;
    /**
     * @ORM\Column(name="postItem_title", type="string", length=80, nullable=false)
     */
    protected $title;
    /**
     * @ORM\Column(name="postItem_body", type="string", length=480)
     */
    protected $body;
    /**
     * @ORM\Column(name="postItem_createDttm", type="datetimetz", nullable=false)
     */
    protected $createDttm;
    /**
     * @ORM\Column(name="postItem_isActive", type="boolean", nullable=false, options={"default":1})
     */
    protected $isActive;

    /**
     * @ORM\ManyToOne(targetEntity="GeoFilter", inversedBy="posts", cascade={"persist", "remove"})
     * @ORM\JoinColumn(name="geoFilter_id", referencedColumnName="geoFilter_id", nullable=false)
     */
    protected $geoFilter;
    //</editor-fold>
    //<editor-fold desc="validation">
    public static function loadValidatorMetadata(ClassMetadata $metadata) //TODO: finnish adding constraints.
    {
        $metadata->addPropertyConstraint('title', new NotBlank());
        $metadata->addPropertyConstraint('body', new NotBlank());
    }
    //</editor-fold>
    //<editor-fold desc="generated accessors">
    /**
     * Constructor
     */
    public function __construct()
    {
        $this->setCreateDttm(new \DateTime('now'));
        $this->pics = new \Doctrine\Common\Collections\ArrayCollection();
        $this->comments = new \Doctrine\Common\Collections\ArrayCollection();
        $this->offers = new \Doctrine\Common\Collections\ArrayCollection();
        $this->setIsActive(true);
    }

    /**
     * @ORM\PrePersist()
     * @ORM\PreUpdate()
     */
    public function preUpload() //TODO:make sure removed pics are being removed from the database and server
    {
        /*
        for ($i = 0; $i < $this->getPics()->count(); $i++) {
            $pic = $this->getPics()->toArray()[$i];
            if ($pic->getFile() === null) {
                $this->getPics()->removeElement($pic);
                $i--;
            }
        }
        */
        foreach ($this->getPics() as $pic) {
            $pic->setPostItem($this); //TODO:cleanup
        }
    }

    /**
     * @param \Doctrine\Common\Collections\ArrayCollection $pics
     */
    public function resetPics($pics = null)
    {
        if ($pics === null) {
            $this->pics = new \Doctrine\Common\Collections\ArrayCollection();
        } else {
            $this->pics = $pics;
        }
    }

    /**
     * @ORM\PostPersist()
     * @ORM\PostUpdate()
     */
    public function postUpload()
    {
        foreach ($this->getPics() as $pic) {
            $pic->setPostItem($this); //TODO:cleanup
        }
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
     * Set title
     *
     * @param string $title
     * @return PostItem
     */
    public function setTitle($title)
    {
        $this->title = $title;

        return $this;
    }

    /**
     * Get title
     *
     * @return string
     */
    public function getTitle()
    {
        return $this->title;
    }

    /**
     * Set body
     *
     * @param string $body
     * @return PostItem
     */
    public function setBody($body)
    {
        $this->body = $body;

        return $this;
    }

    /**
     * Get body
     *
     * @return string
     */
    public function getBody()
    {
        return $this->body;
    }

    /**
     * Set createDttm
     *
     * @param \DateTime $createDttm
     * @return PostItem
     */
    public function setCreateDttm($createDttm)
    {
        $this->createDttm = $createDttm;

        return $this;
    }

    /**
     * Get createDttm
     *
     * @return \DateTime
     */
    public function getCreateDttm()
    {

        $stringDate = date_format( $this->createDttm, "m-d-Y H:i");

        return $stringDate;
    }

    /**
     * Set isActive
     *
     * @param boolean $isActive
     * @return PostItem
     */
    public function setIsActive($isActive)
    {
        $this->isActive = $isActive;

        return $this;
    }

    /**
     * Get isActive
     *
     * @return boolean
     */
    public function getIsActive()
    {
        return $this->isActive;
    }

    /**
     * Set user
     *
     * @param \TradeOwl\TradeBundle\Entity\User $user
     * @return PostItem
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
     * Add pics
     *
     * @param \TradeOwl\TradeBundle\Entity\PostPicture $pics
     * @return PostItem
     */
    public function addPic(\TradeOwl\TradeBundle\Entity\PostPicture $pics)
    {
        $this->getPics()->add($pics);
//        $this->pics[] = $pics;

        return $this;
    }

    /**
     * Remove pics
     *
     * @param \TradeOwl\TradeBundle\Entity\PostPicture $pics
     */
    public function removePic(\TradeOwl\TradeBundle\Entity\PostPicture $pics)
    {
        $this->pics->removeElement($pics);
    }

    /**
     * Get pics
     *
     * @return \Doctrine\Common\Collections\Collection
     */
    public function getPics()
    {
        return $this->pics;
    }

    /**
     * Return pics
     *
     * @return  \TradeOwl\TradeBundle\Entity\PostPicture $pics
     *
     */
    public function returnPics()
    {
        return $this->pics;
    }


    /**
     * Add comments
     *
     * @param \TradeOwl\TradeBundle\Entity\Comment $comments
     * @return PostItem
     */
    public function addComment(\TradeOwl\TradeBundle\Entity\Comment $comments)
    {
        $this->comments[] = $comments;

        return $this;
    }

    /**
     * Remove comments
     *
     * @param \TradeOwl\TradeBundle\Entity\Comment $comments
     */
    public function removeComment(\TradeOwl\TradeBundle\Entity\Comment $comments)
    {
        $this->comments->removeElement($comments);
    }

    /**
     * Get comments
     *
     * @return \Doctrine\Common\Collections\Collection
     */
    public function getComments()
    {
        return $this->comments;
    }

    /**
     * Add offers
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOffer $offers
     * @return PostItem
     */
    public function addOffer(\TradeOwl\TradeBundle\Entity\TradeOffer $offers)
    {
        $this->offers[] = $offers;

        return $this;
    }

    /**
     * Remove offers
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOffer $offers
     */
    public function removeOffer(\TradeOwl\TradeBundle\Entity\TradeOffer $offers)
    {
        $this->offers->removeElement($offers);
    }

    /**
     * Get offers
     *
     * @return \Doctrine\Common\Collections\Collection
     */
    public function getOffers()
    {
        return $this->offers;
    }
    //</editor-fold>

    /**
     * Set geoFilter
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoFilter $geoFilter
     * @return PostItem
     */
    public function setGeoFilter(\TradeOwl\TradeBundle\Entity\GeoFilter $geoFilter = null)
    {
        $this->geoFilter = $geoFilter;

        return $this;
    }

    /**
     * Get geoFilter
     *
     * @return \TradeOwl\TradeBundle\Entity\GeoFilter 
     */
    public function getGeoFilter()
    {
        return $this->geoFilter;
    }
}
