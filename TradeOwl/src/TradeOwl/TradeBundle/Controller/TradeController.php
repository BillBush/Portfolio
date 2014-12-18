<?php

namespace TradeOwl\TradeBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\Config\Definition\Exception\Exception;
use Symfony\Component\HttpFoundation\Request;
use TradeOwl\TradeBundle\Entity\TradeOffer;
use TradeOwl\TradeBundle\Entity\TradeOfferChain;
use TradeOwl\TradeBundle\Entity\User;
use Symfony\Component\HttpFoundation\JsonResponse;

class TradeController extends Controller
{
    private function displayAction(User $sender, User $receiver,
                                   $nextScreen = 'trade_owl_trade_Default',
                                   $prevOfferId = '00', Request $request){
        $em = $this->getDoctrine()->getManager();

        $repo = $em->getRepository('TradeOwlTradeBundle:PostItem');
        $senderPosts = $repo->findByuser($sender);
        $receiverPosts = $repo->findByuser($receiver);

        return $this->render('TradeOwlTradeBundle:Offer:make_offer.html.twig',
            array('id' => $sender->getId(), 'traderId' => $receiver->getId(),
                'posts' => $senderPosts, 'traderPosts' => $receiverPosts,
                'nextScreen' => $nextScreen, 'prevOfferId' => $prevOfferId));
    }

    public function offerAction($tradeItem, Request $request)
    {
        $em = $this->getDoctrine()->getManager();

        $sender = $em->getRepository('TradeOwlTradeBundle:User')
            ->find($this->get('security.context')
            ->getToken()->getUser()->getId());

        $receiver = $em->getRepository('TradeOwlTradeBundle:User')
            ->find($em->getRepository('TradeOwlTradeBundle:PostItem')
                ->find($tradeItem)->getUser()->getId());

        return $this->displayAction($sender, $receiver, 'trade_owl_trade_Default',
            '00',$request);
    }

    public function closeAction(Request $request){
        $resp['err'] = '';
        $em = $this->getDoctrine()->getManager();
        $closer = $this->get('security.context')->getToken()->getUser();
        if(!is_object($closer) || is_null($closer) || !$closer){
            $resp['err'] = "You must log back in order to perform this action!";
            return new jsonresponse($resp);
        }
        $closer = $em->getRepository('TradeOwlTradeBundle:User')->find($closer->getId());
        $offer = $em->getRepository('TradeOwlTradeBundle:TradeOffer')->find($request->get('offerId'));
        $chain = $offer->getChain();
        $action = $request->get('operation');
        $chain->setUserClosing($closer);
        if($action == "accept"){
            $chain->setStatus(1);
            foreach($offer->getPosts() as $post){
                foreach($post->getOffers() as $offer){
                    if ($offer->getChain()->getStatus() == 0){
                        $offer->getChain()->setStatus(2);
                        $offer->getChain()->setUserClosing($post->getUser());
                        $em->persist($offer->getChain());
                    }
                }
                if ($post->getUser()->getId() == $offer->getUserRecieving()->getId()){
                    $post->setUser($offer->getUserSending());
                } elseif ($post->getUser()->getId() == $offer->getUserSending()->getId()){
                    $post->setUser($offer->getUserRecieving());
                }
                $em->persist($post);
            }
        } else if ($action == "reject"){
            $chain->setStatus(2);
        }
        $em->persist($chain);
        $em->flush();
        $resp['dttm'] = $chain->getCloseDttmStr();
        $resp['chainId'] = $chain->getId();
        return new jsonresponse($resp);
    }

    public function counterAction($offerId, Request $request){
        $em = $this->getDoctrine()->getManager();
        $sender = $em->getRepository('TradeOwlTradeBundle:User')
            ->find($this->get('security.context')
                ->getToken()->getUser()->getId());
        $offer = $em->getRepository('TradeOwlTradeBundle:TradeOffer')
            ->find($offerId);
        $receiver = null;
        if($sender->getId() == $offer->getUserRecieving()->getId()){
            $receiver = $offer->getUserSending();
        }else if($sender->getId() == $offer->getUserSending()->getId()){
            $receiver = $offer->getUserRecieving();
        }
        return $this->displayAction($sender, $receiver,
            'trade_owl_trade_profile_view', $offer->getId(), $request);
    }

    public function submitOfferAction($nextScreen = 'trade_owl_trade_Default',
                                      $prevOfferId = 00, Request $request)
    {
        $em = $this->getDoctrine()->getManager();

        $senderId = $request->get('You');
        $senderPostIds = $request->get($senderId);
        $receiverId = $request->get('Them');
        $receiverPostIds = $request->get($receiverId);

        $sender = $em->getRepository('TradeOwlTradeBundle:User')->find($senderId);
        $receiver = $em->getRepository('TradeOwlTradeBundle:User')->find($receiverId);

        $tradeOffer = new TradeOffer();
        $tradeOfferChain = null;
        if ($prevOfferId == 00){
            $tradeOfferChain = new TradeOfferChain();
            $tradeOfferChain->setUserInitiating($sender);
            $tradeOfferChain->setUserApproached($receiver);
            $tradeOffer->setChainSeq(1);
        } else {
            $prevOffer = $em->getRepository('TradeOwlTradeBundle:TradeOffer')
                ->find($prevOfferId);
            $tradeOffer->setChainSeq($prevOffer->getChainSeq() + 1);
            $tradeOfferChain = $prevOffer->getChain();
        }
        $tradeOffer->setUserRecieving($receiver);
        $tradeOffer->setUserSending($sender);
        $tradeOffer->setChain($tradeOfferChain);

        foreach ($senderPostIds as $postId) {//TODO fix this
            $tradeOffer->addPost($em->getRepository('TradeOwlTradeBundle:PostItem')->find($postId));
        }
        foreach ($receiverPostIds as $postId) {
            $tradeOffer->addPost($em->getRepository('TradeOwlTradeBundle:PostItem')->find($postId));
        }
        $tradeOfferChain->addOffer($tradeOffer);
        if($prevOfferId == 00){
            $em->persist($tradeOfferChain);
        } else {
            $em->persist($tradeOffer);
        }
        $em->flush();

        $this->get('session')->getFlashBag()->add(
            'notice',
            'Your trade was offered!'
        );
        if ($nextScreen == 'trade_owl_trade_profile_view'){
            return $this->redirect($this->generateUrl($nextScreen) . '/' . $sender->getId());
        }
        return $this->redirect($this->generateUrl($nextScreen));
    }
}
