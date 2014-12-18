<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 8/25/14
 * Time: 9:46 AM
 */

namespace TradeOwl\TradeBundle\Controller;

use Doctrine\Common\Collections\ArrayCollection;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Security\Acl\Exception\Exception;
use Symfony\Component\Security\Core\SecurityContextInterface;
use Symfony\Component\Validator\Constraints\Collection;
use TradeOwl\TradeBundle\Entity\GeoFilter;
use TradeOwl\TradeBundle\Entity\PostItem;
use TradeOwl\TradeBundle\Form\PostItemType;
use TradeOwl\TradeBundle\Form\GeoFilterType;
use TradeOwl\TradeBundle\Entity\PostPicture;

class PostController extends Controller
{
    public function display(Request $request, $message = "", $id = null, $action = 'trade_owl_trade_post_create')
    {
        $em = $this->getDoctrine()->getManager();
        $user = $this->container->get('security.context')->getToken()->getUser();
        $post = new PostItem();
        $post->setUser($user);
        if (is_null($user->getPicNrml())){
            $defPic = $em->getRepository('TradeOwlTradeBundle:PostPicture')->find(12);
            $newPic = new PostPicture();
            $newPic->copy($defPic);
            $user->setPicNrml($newPic);
        }

        //If an id is passed in then we're either modifying or viewing a post
        if ($id !== null) {
            $post = $em->getRepository('TradeOwlTradeBundle:PostItem')->find($id); //get the repository
            if (!$post) {
                throw $this->createNotFoundException('Post not found!'); //TODO:handle more gracefully
            }
            foreach ($post->getPics() as $pic) {
                $pic->setPostItem($post); //TODO: cleanup
            }
        }

        foreach ($user->getChainsStartedByMe() as $chain){
            $chains[] = $chain;
        }


        if ($request->getMethod() == 'POST') {
            $previousPics = new ArrayCollection();
            if ($action === 'trade_owl_trade_post_edit') {
                foreach ($post->getPics() as $pic) {
                    $previousPics->add($pic);
                }
            }
//            $post->setGeoFilter(new GeoFilter());
            $form = $this->createForm(new PostItemType($post->getUser()), $post);
//            throw new Exception($post->getGeoFilter()->getId());
            if ($post->getUser()->getId() !== $user->getId()) {
                throw new Exception("User not authorized to edit post!"); //TODO:handle more gracefully
            }
            $form->handleRequest($request);
            if ($form->isValid()) {
                foreach ($previousPics as $pic) {
                    if (!$post->getPics()->contains($pic)) {
                        $em->remove($pic);
                    }
                }
                foreach ($post->getPics() as $pic) {
                    $pic->setPostItem($post); //TODO:cleanup
                }
                $em->persist($post);
                $em->flush();
                return $this->redirect($this->generateUrl('trade_owl_trade_post_success'));
            }
        } else if ($request->getMethod() == 'GET') {

            $pics = $post->getPics();
            $post->resetPics();
            $form = $this->createForm(new PostItemType($post->getUser()), $post);
            $post->resetPics($pics);

            //begin geo
            $filter = new GeoFilter();
            $filter->setUser($user);
            $geoForm = $this->createForm(new GeoFilterType(), $filter);
            //end geo
            return $this->render('TradeOwlTradeBundle:Post:post_edit.html.twig', array('postMessage' => $message,
                'form' => $form->createView(), 'pics' => $post->getPics(), 'action' => $action, 'postId' => $id,
                'geoForm' => $geoForm->createView(), 'polygons' => $filter->getPolygons(),
                'geoAction' => 'trade_owl_trade_geo_save'));
        }
    }

    public function createAction(Request $request, $message = "")
    {
        return $this->display($request);
    }

    public function successAction(Request $request)
    {
        return $this->display($request, "Your item was posted successfully!");
    }

    public function editAction(Request $request, $id)
    {
        return $this->display($request, "", $id, 'trade_owl_trade_post_edit');
    }

    public function viewAction($id)
    {
        $this->get('twig')->addExtension(new \Twig_Extension_Debug);
        $em = $this->getDoctrine()->getManager(); //get the Entity Manager
        $post = $em->getRepository('TradeOwlTradeBundle:PostItem')->find($id); //get the repository

        if (!$post) {
            throw $this->createNotFoundException('No posts found ');
        }

        $comments = array();
        foreach ($post->getComments() as $comment){
            $i=0;
            for(;$i<count($comments);$i++){
                if ($comments[$i]->getCreateDttm() > $comment->getCreateDttm()){
                    break;
                }
            }
            if ($i < count($comments)){
                $tail = array_splice($comments, $i);
                $comments[] = $comment;
                foreach($tail as $val){
                    $comments[] = $val;
                }
            } else {
                $comments[] = $comment;
            }
        }
        $post->getComments()->clear();
        foreach($comments as $comment){
            $post->addComment($comment);
        }

        if(is_null($post->getUser()->getPicNrml())){
            $defPic = $em->getRepository('TradeOwlTradeBundle:PostPicture')->find(12);
            $newPic = new PostPicture();
            $newPic->copy($defPic);
            $post->getUser()->setPic(array($newPic));
        }

        return $this->render('TradeOwlTradeBundle:Post:post_view.html.twig', array(
            'post' => $post
        ));
    }


}