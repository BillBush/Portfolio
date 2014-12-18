<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 8/23/14
 * Time: 7:30 AM
 */

namespace TradeOwl\TradeBundle\Entity;


use Doctrine\ORM\Mapping as ORM;
use Symfony\Component\Validator\Constraints\DateTime;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblTradeOffer", indexes={@ORM\Index(name="chain_idx", columns={"tradeOfferChain_id", "tradeOffer_chainSeq"})})
 * @ORM\ HasLifecycleCallBacks
 */
class TradeOffer {

    /**
     * @ORM\Id
     * @ORM\Column(name="tradeOffer_id", type="integer")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;

    /**
     * @ORM\ManyToMany(targetEntity="PostItem", inversedBy="offers", cascade={"persist"})
     * @ORM\JoinTable(name="tblPostItem_tblTradeOffer",
     *      joinColumns={@ORM\JoinColumn(name="tradeOffer_id", referencedColumnName="tradeOffer_id")},
     *      inverseJoinColumns={@ORM\JoinColumn(name="postItem_id", referencedColumnName="postItem_id")})
     */
    protected $posts;

    /**
     * @ORM\ManyToOne(targetEntity="User", fetch="EAGER")
     * @ORM\JoinColumn(name="user_idSending", referencedColumnName="user_id")
     */
    protected $userSending;

    /**
     * @ORM\ManyToOne(targetEntity="User", fetch="EAGER")
     * @ORM\JoinColumn(name="user_idRecieving", referencedColumnName="user_id")
     */
    protected $userRecieving;

    /**
     * @ORM\ManyToOne(targetEntity="TradeOfferChain", fetch="EXTRA_LAZY", inversedBy="offers", cascade={"persist"})
     * @ORM\JoinColumn(name="tradeOfferChain_id", referencedColumnName="tradeOfferChain_id")
     */
    protected $chain;

    /**
     * @ORM\Column(name="tradeOffer_chainSeq", type="integer", nullable=false)
     */
    protected $chainSeq;

    /**
     * @ORM\Column(name="tradeOffer_createDttm", type="datetimetz", nullable=false)
     */
    protected $createDttm;

    /**
     * Constructor
     */
    public function __construct()
    {
        $this->setCreateDttm(new \DateTime('now'));
        $this->posts = new \Doctrine\Common\Collections\ArrayCollection();
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
     * Set createDttm
     *
     * @param \DateTime $createDttm
     * @return TradeOffer
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
        return $this->createDttm;
    }

    public function getCreateDttmStr(){
        return date_format( $this->getCreateDttm(), "m-d-Y H:i");
    }

    /**
     * Add posts
     *
     * @param \TradeOwl\TradeBundle\Entity\PostItem $posts
     * @return TradeOffer
     */
    public function addPost(\TradeOwl\TradeBundle\Entity\PostItem $posts)
    {
        $posts->addOffer($this);
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
     * Set chainSeq
     *
     * @param integer $chainSeq
     * @return TradeOffer
     */
    public function setChainSeq($chainSeq)
    {
        $this->chainSeq = $chainSeq;

        return $this;
    }

    /**
     * Get chainSeq
     *
     * @return integer 
     */
    public function getChainSeq()
    {
        return $this->chainSeq;
    }

    /**
     * Set userSending
     *
     * @param \TradeOwl\TradeBundle\Entity\User $userSending
     * @return TradeOffer
     */
    public function setUserSending(\TradeOwl\TradeBundle\Entity\User $userSending = null)
    {
        $this->userSending = $userSending;

        return $this;
    }

    /**
     * Get userSending
     *
     * @return \TradeOwl\TradeBundle\Entity\User 
     */
    public function getUserSending()
    {
        return $this->userSending;
    }

    /**
     * Set userRecieving
     *
     * @param \TradeOwl\TradeBundle\Entity\User $userRecieving
     * @return TradeOffer
     */
    public function setUserRecieving(\TradeOwl\TradeBundle\Entity\User $userRecieving = null)
    {
        $this->userRecieving = $userRecieving;

        return $this;
    }

    /**
     * Get userRecieving
     *
     * @return \TradeOwl\TradeBundle\Entity\User 
     */
    public function getUserRecieving()
    {
        return $this->userRecieving;
    }

    /**
     * Set chain
     *
     * @param \TradeOwl\TradeBundle\Entity\TradeOfferChain $chain
     * @return TradeOffer
     */
    public function setChain(\TradeOwl\TradeBundle\Entity\TradeOfferChain $chain = null)
    {
        $this->chain = $chain;

        return $this;
    }

    /**
     * Get chain
     *
     * @return \TradeOwl\TradeBundle\Entity\TradeOfferChain 
     */
    public function getChain()
    {
        return $this->chain;
    }

    public function getUserSendingAry(){
        return array($this->getUserSending());
    }

    public function setUserSendingAry($userSending){
        foreach($userSending as $user){
            $this->setUserSending($user);
        }

        return $this;
    }
}
