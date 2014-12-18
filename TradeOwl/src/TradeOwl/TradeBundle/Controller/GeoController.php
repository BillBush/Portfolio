<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 9/15/14
 * Time: 1:50 PM
 */

namespace TradeOwl\TradeBundle\Controller;

use Doctrine\Common\Collections\ArrayCollection;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Security\Core\SecurityContextInterface;
use Symfony\Component\Validator\Constraints\Collection;
use TradeOwl\TradeBundle\Entity\GeoFilter;
use TradeOwl\TradeBundle\Entity\GeoPolygon;
use TradeOwl\TradeBundle\Entity\GeoPoint;
use TradeOwl\TradeBundle\Form\GeoFilterType;
use Symfony\Component\HttpFoundation\JsonResponse;


class GeoController extends Controller
{
    public function saveAction(Request $request){
        $formFilter = $request->get("geo_filter");
        $filter = new GeoFilter();
        foreach ($formFilter['polygons'] as $formPoly){
            $polygon = new GeoPolygon();
            foreach($formPoly['points'] as $formPoint){
                $point = new GeoPoint();
                $point->setLat($formPoint['lat']);
                $point->setLng($formPoint['lng']);
                $polygon->addPoint($point);
            }
            $filter->addPolygon($polygon);
        }
        $em = $this->getDoctrine()->getManager();
        $user = $em->getRepository('TradeOwlTradeBundle:User')
            ->find($this->get('security.context')
            ->getToken()->getUser()->getId());
        $filter->setUser($user);
        $filter->setName($formFilter['name']);
        $em->persist($filter);
        $em->flush();
        $retFilter['id'] = $filter->getId();
        $retFilter['name'] = $filter->getName();
        return new jsonresponse($retFilter);
    }
}