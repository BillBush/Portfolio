<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 8/22/14
 * Time: 3:39 PM
 */

namespace TradeOwl\TradeBundle\Entity;

use Doctrine\ORM\Mapping as ORM;
use Doctrine\Common\PropertyChangedListener;
use Doctrine\Common\NotifyPropertyChanged;
use Symfony\Component\HttpFoundation\File\UploadedFile;
use Symfony\Component\Validator\Constraints as Assert;


/**
 * @ORM\Entity
 * @ORM\Table(name="tblPostPicture")
 * @ORM\HasLifecycleCallbacks
 */
class PostPicture
{
    private $temp;//TODO: fix this janky nonsense

    /**
     * @ORM\Id
     * @ORM\Column(type="integer", name="postPic_id")
     * @ORM\GeneratedValue(strategy="IDENTITY")
     */
    protected $id;

    /**
     * @ORM\Column(name="postPic_filePath",type="string", length=255, nullable=false)
     */
    protected $path; //TODO:add logic for subdirectories

    /**
     * @ORM\ManyToOne(targetEntity="PostItem",fetch="EXTRA_LAZY", inversedBy="pics")
     * @ORM\JoinColumn(name="postItem_id", referencedColumnName="postItem_id", nullable=true)
     */
    protected $postItem;

    /**
     * @Assert\Image(maxSize="6000000")
     */
    private $file; //TODO:add more validation assertions

    public function copy(\TradeOwl\TradeBundle\Entity\PostPicture $pic){
        $this->setFile($pic->getFile());
        $this->setPath($pic->getPath());
    }
    /**
     * @param UploadedFile $file
     */
    public function setFile(UploadedFile $file = null)
    {
        $this->file = $file;
        if (isset($this->path)) {
            $this->temp = $this->path;
            $this->path = null;
        } else {
            $this->path = 'initial';
        }
    }

    /**
     * @return UploadedFile
     */
    public function getFile()
    {
        return $this->file;
    }

    /**
     * @ORM\PrePersist()
     * @ORM\PreUpdate()
     */
    public function preUpload()
    {
        if ($this->getId() == 12){
            throw new \Exception("Attempted to change default user pic!");
        }
        if (null !== $this->getFile()) {
            $filename = sha1(uniqid(mt_rand(), true));
            $this->path = $filename . '.' . $this->getFile()->guessExtension();
        }
    }

    public function getAbsolutePath()
    {
        return null === $this->path
            ? null
            : $this->getUploadRootDir() . '/' . $this->path;
    }


    public function getWebPath()
    {
        return null === $this->path
            ? null
            : $this->getUploadDir() . '/' . $this->path;
    }

    public function getThumbWebPath(){
        return null === $this->path
            ? null
            : $this->getUploadDir() . '/' . $this->getThumbPath();
    }

    public function getSourceWebPath(){
        return '/'.$this->getWebPath();
    }

    public function getThumbPath(){
        return 'thm'.$this->getPath();
    }

    public function getThumbSourceWebPath(){
        return '/'.$this->getThumbWebPath();
    }

    protected function getUploadRootDir()
    {
        return __DIR__ . '/../../../../web/' . $this->getUploadDir();
    }

    protected function getUploadDir()
    {
        return 'uploads/media';
    }

    /**
     * @ORM\PostPersist()
     * @ORM\PostUpdate()
     */
    public function upload()
    {
        if (null === $this->getFile()) {
            return;
        }
        $ext = $this->getFile()->guessExtension();
        $fullpath = $this->getUploadRootDir() . '/' . $this->path;

        $this->getFile()->move($this->getUploadRootDir(), $this->path);

        $w = 300;
        $h = 300;
        $thumb = ImageCreateTrueColor($w, $h);
        ImageColorTransparent($thumb, ImageColorAllocateAlpha($thumb, 0, 0, 0, 127));
        ImageAlphaBlending($thumb, false);
        ImageSaveAlpha($thumb, true);
        $image = "";
        if ($ext === "png") {
            $image = imagecreatefrompng($fullpath);
        } elseif ($ext === "gif") {
            $image = imagecreatefromgif($fullpath);
        } elseif ($ext === "jpeg" || $ext === "jpg") {
            $image = imagecreatefromjpeg($fullpath);
        } else {
            throw new Exception($ext); //TODO:handle this more gracefully
        }
        $x = ImageSX($image);
        $y = ImageSY($image);
        $scale = min($x / $w, $y / $h);
        ImageCopyResampled($thumb, $image, 0, 0, ($x - ($w * $scale)) / 2, ($y - ($h * $scale)) / 2, $w, $h, $w * $scale, $h * $scale);
        ImagePNG($thumb, $this->getUploadRootDir() . '/' . $this->getThumbPath());

        if (isset($this->temp)) {
            unlink($this->getUploadRootDir() . '/' . $this->temp);
            $this->temp = null;
        }
        $this->file = null;
    }

    /**
     * @ORM\PostRemove()
     */
    public function removeUpload()
    {
        if ($file = $this->getAbsolutePath()) {
            unlink($file);
            unlink($this->getUploadRootDir() . '/' . $this->getThumbPath());
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
     * Set path
     *
     * @param string $path
     * @return PostPicture
     */
    public function setPath($path)
    {
        $this->path = $path;

        return $this;
    }

    /**
     * Get path
     *
     * @return string
     */
    public function getPath()
    {
        return $this->path;
    }

    /**
     * Set postItem
     *
     * @param \TradeOwl\TradeBundle\Entity\PostItem $postItem
     * @return PostPicture
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
     * Get ImgDisplay
     *
     * @return string
     */

    public function getImgDisplay()
    {


        if ($this->id) {
            return $this->getUploadDir() . $this->getPostItem();
        } else {

            return "No pic";
        }
    }

    /**
     * Constructor
     */
    public function __construct()
    {
    }
}
