<?php
namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use FOS\UserBundle\Model\User as BaseUser;
use TradeOwl\TradeBundle\Entity\PostPicture;
use TradeOwl\TradeBundle\Entity\UserRating;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblUser")
 * @ORM\HasLifecycleCallbacks
 */
class User extends BaseUser
{
    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="user_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;

    /**
     * @ORM\OneToMany(targetEntity="PostItem", mappedBy="user", fetch="EXTRA_LAZY")
     */
    protected $posts;

    /**
     * @ORM\OneToMany(targetEntity="GeoFilter", mappedBy="user", fetch="EXTRA_LAZY")
     */
    protected $geoFilters;

    /**
     * @ORM\OneToMany(targetEntity="Comment", mappedBy="user", fetch="EXTRA_LAZY")
     */
    protected $comments;

    /**
     * @ORM\OneToMany(targetEntity="TradeOfferChain", mappedBy="userInitiating", fetch="LAZY")
     */
    protected $chainsStartedByMe;

    /**
     * @ORM\OneToMany(targetEntity="TradeOfferChain", mappedBy="userApproached", fetch="LAZY")
     */
    protected $chainsStartedByOthers;

    /**
     * @ORM\ManyToOne(targetEntity="PostPicture", fetch="LAZY", cascade={"persist"})
     * @ORM\JoinColumn(name="postPic_id", referencedColumnName="postPic_id")
     **/
    protected $pic;

    /**
     * @ORM\OneToMany(targetEntity="UserRating", mappedBy="rater", fetch="EXTRA_LAZY")
     * @ORM\JoinColumn(name="userRating_rater", referencedColumnName="user_id");
     */
    protected $ratingsGiven;

    /**
     * @ORM\OneToMany(targetEntity="UserRating", mappedBy="rated", fetch="EXTRA_LAZY")
     * @ORM\JoinColumn(name="userRating_rated", referencedColumnName="user_id");
     */
    protected $ratingsReceived;

    public function getAvgRating(){
        $avg = 0;
        foreach($this->getRatingsReceived() as $rating){
            $avg += $rating->getRating();
        }
        if ($avg > 0){
            $avg /= $this->getRatingsReceived()->count();
        }
        return $avg;
    }

    /**
     * @param User $user
     * @return mixed
     */
    public function getRatingGiven(User $user){
        foreach($this->getRatingsGiven() as $rating){
            if($rating->getRated()->getId() == $user->getId()){
                return $rating;
            }
        }
    }

    /**
     * @ORM\PrePersist()
     * @ORM\PreUpdate()
     */
    public function preUpload()
    {
        if (!is_null($this->getPicNrml()) && $this->getPicNrml()->getId() == 12){
            $newPic = new PostPicture();
            $newPic->copy($this->getPicNrml());
            $this->setPicNrml($newPic);
        }
    }

    public function __construct()
    {
        parent::__construct();
        // your own logic
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
     * Add posts
     *
     * @param \TradeOwl\TradeBundle\Entity\PostItem $posts
     * @return User
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

    /**
     * Add comments
     *
     * @param \TradeOwl\TradeBundle\Entity\Comment $comments
     * @return User
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
     * Add geoFilters
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoFilter $geoFilters
     * @return User
     */
    public function addGeoFilter(\TradeOwl\TradeBundle\Entity\GeoFilter $geoFilters)
    {
        $this->geoFilters[] = $geoFilters;

        return $this;
    }

    /**
     * Remove geoFilters
     *
     * @param \TradeOwl\TradeBundle\Entity\GeoFilter $geoFilters
     */
    public function removeGeoFilter(\TradeOwl\TradeBundle\Entity\GeoFilter $geoFilters)
    {
        $this->geoFilters->removeElement($geoFilters);
    }

    /**
     * Get geoFilters
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getGeoFilters()
    {
        return $this->geoFilters;
    }

    /**
     * Add chainsStartedByMe
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByMe
     * @return User
     */
    public function addChainsStartedByMe(\TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByMe)
    {
        $this->chainsStartedByMe[] = $chainsStartedByMe;

        return $this;
    }

    /**
     * Remove chainsStartedByMe
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByMe
     */
    public function removeChainsStartedByMe(\TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByMe)
    {
        $this->chainsStartedByMe->removeElement($chainsStartedByMe);
    }

    /**
     * Get chainsStartedByMe
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getChainsStartedByMe()
    {
        return $this->chainsStartedByMe;
    }

    /**
     * Add chainsStartedByOthers
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByOthers
     * @return User
     */
    public function addChainsStartedByOther(\TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByOthers)
    {
        $this->chainsStartedByOthers[] = $chainsStartedByOthers;

        return $this;
    }

    /**
     * Remove chainsStartedByOthers
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByOthers
     */
    public function removeChainsStartedByOther(\TradeOwl\TradeBundle\Entity\TradeOfferChain $chainsStartedByOthers)
    {
        $this->chainsStartedByOthers->removeElement($chainsStartedByOthers);
    }

    /**
     * Get chainsStartedByOthers
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getChainsStartedByOthers()
    {
        return $this->chainsStartedByOthers;
    }

    /**
     * Set pic
     *
     * The array option is for compatibility with symfony form types
     *
     * @param array $ary
     * @throws \Exception
     */
    public function setPic(array $ary = null)
    {
        $this->pic = $ary[0];

        return $this;
    }

    public function setPicNrml(\TradeOwl\TradeBundle\Entity\PostPicture $pic)
    {
        $this->pic = $pic;

        return $this;
    }

    /**
     * Get pic
     *
     * @return \TradeOwl\TradeBundle\Entity\PostPicture
     */
    public function getPic()
    {
            return array($this->pic);
    }

    public function getPicNrml()//TODO:fix this jankiness
    {
        return $this->pic;
    }

    /**
     * Add ratingsGiven
     *
     * @param \TradeOwl\TradeBundle\Entity\UserRating $ratingsGiven
     * @return User
     */
    public function addRatingsGiven(\TradeOwl\TradeBundle\Entity\UserRating $ratingsGiven)
    {
        $this->ratingsGiven[] = $ratingsGiven;

        return $this;
    }

    /**
     * Remove ratingsGiven
     *
     * @param \TradeOwl\TradeBundle\Entity\UserRating $ratingsGiven
     */
    public function removeRatingsGiven(\TradeOwl\TradeBundle\Entity\UserRating $ratingsGiven)
    {
        $this->ratingsGiven->removeElement($ratingsGiven);
    }

    /**
     * Get ratingsGiven
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getRatingsGiven()
    {
        return $this->ratingsGiven;
    }

    /**
     * Add ratingsReceived
     *
     * @param \TradeOwl\TradeBundle\Entity\UserRating $ratingsReceived
     * @return User
     */
    public function addRatingsReceived(\TradeOwl\TradeBundle\Entity\UserRating $ratingsReceived)
    {
        $this->ratingsReceived[] = $ratingsReceived;

        return $this;
    }

    /**
     * Remove ratingsReceived
     *
     * @param \TradeOwl\TradeBundle\Entity\UserRating $ratingsReceived
     */
    public function removeRatingsReceived(\TradeOwl\TradeBundle\Entity\UserRating $ratingsReceived)
    {
        $this->ratingsReceived->removeElement($ratingsReceived);
    }

    /**
     * Get ratingsReceived
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getRatingsReceived()
    {
        return $this->ratingsReceived;
    }
}
