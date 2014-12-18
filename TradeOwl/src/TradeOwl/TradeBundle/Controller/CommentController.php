<?php
/**
 * Created by PhpStorm.
 * User: bill
 * Date: 11/17/14
 * Time: 1:51 PM
 */

namespace TradeOwl\TradeBundle\Controller;

use TradeOwl\TradeBundle\Entity\Comment;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\JsonResponse;

class CommentController extends Controller {

    public function createAction(Request $request)
    {
        $retVal=null;
        $user = $this->container->get('security.context')->getToken()->getUser();
        if(!is_object($user) || is_null($user)){
            $retVal['errMsg']="Must login or register to post comments!";
        }
        $em = $this->getDoctrine()->getManager();
        $user = $em->getRepository('TradeOwlTradeBundle:User')->find($user->getId());
        if (is_null($user->getPicNrml())){
            $defPic = $em->getRepository('TradeOwlTradeBundle:PostPicture')->find(12);
            $user->setPic(array($defPic));
        }
        $post = $em->getRepository('TradeOwlTradeBundle:PostItem')->find($request->get('postId'));

        $comment = new Comment();
        $comment->setUser($user);
        $comment->setPostItem($post);
        $comment->setContent($request->get('content'));
        $comment->setShow(false);
        $em->persist($comment);
        $em->flush();
        $retVal['errMsg']="";
        $retVal['user']=$user->getUsername();
        $retVal['userId']=$user->getId();
        $retVal['dttm']=$comment->getCreateDttmStr();
        $retVal['img']=$user->getPicNrml()->getThumbSourceWebPath();
        return new jsonresponse($retVal);
    }
}