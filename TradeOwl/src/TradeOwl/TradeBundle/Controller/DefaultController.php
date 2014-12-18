<?php

namespace TradeOwl\TradeBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use TradeOwl\TradeBundle\Entity\User;
use TradeOwl\TradeBundle\Entity\GeoFilter;
use TradeOwl\TradeBundle\Form\GeoFilterType;

class DefaultController extends Controller
{
    public function indexAction()
    {
        $user = $this->container->get('security.context')->getToken()->getUser();
        $geoArray = array();
        if ($this->container->get('security.context')->isGranted('IS_AUTHENTICATED_REMEMBERED')){
            $Geolist = $user->getGeoFilters();
            $geoArray = array();
            $em = $this->getDoctrine()->getManager();
            $usa = $em->getRepository('TradeOwlTradeBundle:GeoFilter')->find(1);
            $user->addGeoFilter($usa);
            foreach($Geolist as $key => $value) {
                $geoArray[] = array($key => $value);
            }
        }
        //begin geo
        $filter = new GeoFilter();
        if (is_object($user)) {
            $filter->setUser($user);
        }
        $geoForm = $this->createForm(new GeoFilterType(), $filter);
        return $this->render('TradeOwlTradeBundle:Default:index.html.twig', array('geoArray'=>$geoArray,'geoForm' => $geoForm->createView(),
            'polygons' => $filter->getPolygons(), 'geoAction' => 'trade_owl_trade_geo_save'));
        //end geo
    }
}
