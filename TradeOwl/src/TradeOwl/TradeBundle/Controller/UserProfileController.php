<?php
/**
 * Created by PhpStorm.
 * User: bill
 * Date: 11/15/14
 * Time: 9:31 AM
 */
namespace TradeOwl\TradeBundle\Controller;

use Symfony\Component\HttpFoundation\Request;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\Security\Acl\Exception\Exception;
use TradeOwl\TradeBundle\Entity\PostPicture;
use TradeOwl\TradeBundle\Entity\UserRating;
use TradeOwl\TradeBundle\Form\UserProfileType;
use Symfony\Component\HttpFoundation\JsonResponse;

class UserProfileController extends Controller
{

    public function viewAction($id, $message = "", Request $request)
    {
        $em = $this->getDoctrine()->getManager();
        if (!is_object($this->container->get('security.context')->getToken()->getUser())){
            return $this->redirect('/login');
        }
        if (is_null($id) || $id == 0 || $id == '00' || !$id){
            $id = $this->container->get('security.context')->getToken()->getUser()->getId();
        }
        $user = $em->getRepository('TradeOwlTradeBundle:User')->find($id);
        $userForm = $this->createForm(new UserProfileType(), $user);

        if ($request->getMethod() == 'POST') {
            $userForm->handleRequest($request);
            if ($userForm->isValid()) {
                if (is_null($user->getPicNrml())) {
                    $defPic = $em->getRepository('TradeOwlTradeBundle:PostPicture')->find(12);
                    $newPic = new PostPicture();
                    $newPic->copy($defPic);
                    $user->setPicNrml($newPic);
                } elseif ($user->getPicNrml()->getId() == 12){
                    throw new \Exception("Invalid state reached!");
                }
                $em->persist($user);
                $em->flush();
            }
        }

        if (is_null($user->getPicNrml())) {
            $defPic = $em->getRepository('TradeOwlTradeBundle:PostPicture')->find(12);
            $newPic = new PostPicture();
            $newPic->copy($defPic);
            $user->setPicNrml($newPic);
        }

        $chains = array();
        foreach ($user->getChainsStartedByMe() as $chain) {
            $chains[] = $chain;
        }
        foreach ($user->getChainsStartedByOthers() as $chain) {
            $chains[] = $chain;
        }
        $offers = array();
        foreach ($chains as $chain) {
            $offer = $chain->getOffers()[count($chain->getOffers()) - 1];
            $i = 0;
            for (; $i < count($offers); $i++) {
                if ($offers[$i]->getCreateDttm() < $offer->getCreateDttm()) {
                    break;
                }
            }
            if ($i < count($offers)) {
                $tail = array_splice($offers, $i);
                $offers[] = $offer;
                foreach ($tail as $val) {
                    $offers[] = $val;
                }
            } else {
                $offers[] = $offer;
            }
        }
        $offersOpen = array();
        foreach($offers as $offer){
            if($offer->getChain()->getStatus() == 0){
                $offersOpen[] = $offer;
            }
        }
        $offersAccepted = array();
        foreach($offers as $offer){
            if($offer->getChain()->getStatus() == 1){
                $offersAccepted[] = $offer;
            }
        }
        $offersRejected = array();
        foreach($offers as $offer){
            if($offer->getChain()->getStatus() == 2){
                $offersRejected[] = $offer;
            }
        }
        return $this->render('TradeOwlTradeBundle:Profile:user_profile_view.html.twig',
            array('message' => $message, 'pic' => $user->getPicNrml(),
                'userForm' => $userForm->createView(), 'mostRecentOffersOpen' => $offersOpen,
                'mostRecentOffersAccepted' => $offersAccepted,
                'mostRecentOffersRejected' => $offersRejected,
                'userName' => $user->getUsername()
            )
        );
    }

    public function rateAction(Request $request){
        $resp['err'] = '';
        $ratingUser = $this->container->get('security.context')->getToken()->getUser();
        if (!is_object($ratingUser) || is_null($ratingUser)){
            $resp['err']='Your login timed out!  You must log back in to rate this user!';
            return new jsonresponse($resp);
        }
        $em = $this->getDoctrine()->getManager();
        $ratingUser = $em->getRepository('TradeOwlTradeBundle:User')
            ->find($ratingUser->getId());
        $ratedUser = $em->getRepository('TradeOwlTradeBundle:User')
            ->find($request->get('ratedUserId'));
        $newRating = $request->get('newRating');
        foreach($ratedUser->getRatingsReceived() as $rating){
            if ($rating->getRater()->getId() == $ratingUser->getId()){
                $rating->setRating($newRating);
                $em->persist($rating);
                $em->flush();
                $resp['avgRating'] = $ratedUser->getAvgRating();
                return new jsonresponse($resp);
            }
        }
        $rating = new UserRating();
        $rating->setRated($ratedUser);
        $rating->setRater($ratingUser);
        $rating->setRating($newRating);
        $em->persist($rating);
        $em->flush();
        $resp['avgRating'] = $ratedUser->getAvgRating();
        return new jsonresponse($resp);
    }
}