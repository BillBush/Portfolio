<?php
/**
 * Created by PhpStorm.
 * User: bill
 * Date: 11/16/14
 * Time: 2:38 PM
 */

namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * @ORM\Entity
 * @ORM\Table(name="tblUserRating")
 */
class UserRating {
    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="userRating_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;

    /**
     * @ORM\ManyToOne(targetEntity="User", inversedBy="ratingsGiven", fetch="EXTRA_LAZY")
     * @ORM\JoinColumn(name="userRating_rater", referencedColumnName="user_id");
     */
    protected $rater;

    /**
     * @ORM\ManyToOne(targetEntity="User", inversedBy="ratingsReceived", fetch="EXTRA_LAZY")
     * @ORM\JoinColumn(name="userRating_rated", referencedColumnName="user_id");
     */
    protected $rated;

    /**
     * @ORM\Column(type="integer", name="userRating_rating")
     */
    protected $rating;

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
     * Set rater
     *
     * @param \TradeOwl\TradeBundle\Entity\User $rater
     * @return UserRating
     */
    public function setRater(\TradeOwl\TradeBundle\Entity\User $rater = null)
    {
        if ($this->getRater() && is_object($this->getRater()) && !is_null($this->getRater())){
            $this->getRater()->removeRatingsGiven($this);
        }
        $this->rater = $rater;
        $rater->addRatingsGiven($this);

        return $this;
    }

    /**
     * Get rater
     *
     * @return \TradeOwl\TradeBundle\Entity\User 
     */
    public function getRater()
    {
        return $this->rater;
    }

    /**
     * Set rated
     *
     * @param \TradeOwl\TradeBundle\Entity\User $rated
     * @return UserRating
     */
    public function setRated(\TradeOwl\TradeBundle\Entity\User $rated = null)
    {
        if ($this->getRated() && is_object($this->getRated()) && !is_null($this->getRated())){
            $this->getRated()->removeRatingsRecieved($this);
        }
        $this->rated = $rated;
        $rated->addRatingsReceived($this);

        return $this;
    }

    /**
     * Get rated
     *
     * @return \TradeOwl\TradeBundle\Entity\User 
     */
    public function getRated()
    {
        return $this->rated;
    }

    /**
     * Set rating
     *
     * @param integer $rating
     * @return UserRating
     */
    public function setRating($rating)
    {
        $this->rating = $rating;

        return $this;
    }

    /**
     * Get rating
     *
     * @return integer 
     */
    public function getRating()
    {
        return $this->rating;
    }
}
