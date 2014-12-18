<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 8/23/14
 * Time: 7:01 AM
 */

namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblComment")
 */
class Comment {

    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="comment_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;

    /**
     * @ORM\ManyToOne(targetEntity="PostItem", fetch="EXTRA_LAZY", inversedBy="comments")
     * @ORM\JoinColumn(name="postItem_id", referencedColumnName="postItem_id")
     */
    protected $postItem;

    /**
     * @ORM\ManyToOne(targetEntity="User", fetch="EAGER", inversedBy="comments")
     * @ORM\JoinColumn(name="user_id", referencedColumnName="user_id")
     */
    protected $user;

    /**
     * @ORM\Column(name="comment_content", type="string", length=400, nullable=false)
     */
    protected $content;

    /**
     * @ORM\Column(name="comment_createDttm", type="datetimetz", nullable=false)
     */
    protected $createDttm;

    /**
     * @ORM\OneToMany(targetEntity="Comment", mappedBy="parent", fetch="EXTRA_LAZY")
     */
    protected $replies;

    /**
     * @ORM\ManyToOne(targetEntity="Comment", fetch="EXTRA_LAZY", inversedBy="replies")
     * @ORM\JoinColumn(name="comment_idParent", referencedColumnName="comment_id")
     */
    protected $parent;

    /**
     * @ORM\Column(name="comment_show", type="boolean", nullable=false, options={"default":1})
     */
    protected $show;

    /**
     * Constructor
     */
    public function __construct()
    {
        $this->replies = new \Doctrine\Common\Collections\ArrayCollection();
        $this->setCreateDttm(new \DateTime('now'));
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
     * Set content
     *
     * @param string $content
     * @return Comment
     */
    public function setContent($content)
    {
        $this->content = $content;

        return $this;
    }

    /**
     * Get content
     *
     * @return string 
     */
    public function getContent()
    {
        return $this->content;
    }

    /**
     * Set createDttm
     *
     * @param \DateTime $createDttm
     * @return Comment
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

    public function getCreateDttmStr()
    {
        return date_format( $this->getCreateDttm(), "m-d-Y H:i");
    }

    /**
     * Set show
     *
     * @param boolean $show
     * @return Comment
     */
    public function setShow($show)
    {
        $this->show = $show;

        return $this;
    }

    /**
     * Get show
     *
     * @return boolean 
     */
    public function getShow()
    {
        return $this->show;
    }

    /**
     * Set postItem
     *
     * @param \TradeOwl\TradeBundle\Entity\PostItem $postItem
     * @return Comment
     */
    public function setPostItem(\TradeOwl\TradeBundle\Entity\PostItem $postItem = null)
    {
        $this->postItem = $postItem;

        return $this;
    }

    /**
     * Get postItem
     *
     * @return \TradeOwl\TradeBundle\Entity\PostItem 
     */
    public function getPostItem()
    {
        return $this->postItem;
    }

    /**
     * Set user
     *
     * @param \TradeOwl\TradeBundle\Entity\User $user
     * @return Comment
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
     * Add replies
     *
     * @param \TradeOwl\TradeBundle\Entity\Comment $replies
     * @return Comment
     */
    public function addReply(\TradeOwl\TradeBundle\Entity\Comment $replies)
    {
        $this->replies[] = $replies;

        return $this;
    }

    /**
     * Remove replies
     *
     * @param \TradeOwl\TradeBundle\Entity\Comment $replies
     */
    public function removeReply(\TradeOwl\TradeBundle\Entity\Comment $replies)
    {
        $this->replies->removeElement($replies);
    }

    /**
     * Get replies
     *
     * @return \Doctrine\Common\Collections\Collection 
     */
    public function getReplies()
    {
        return $this->replies;
    }

    /**
     * Set parent
     *
     * @param \TradeOwl\TradeBundle\Entity\Comment $parent
     * @return Comment
     */
    public function setParent(\TradeOwl\TradeBundle\Entity\Comment $parent = null)
    {
        $this->parent = $parent;

        return $this;
    }

    /**
     * Get parent
     *
     * @return \TradeOwl\TradeBundle\Entity\Comment 
     */
    public function getParent()
    {
        return $this->parent;
    }
}
