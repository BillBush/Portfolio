<?php
/**
 * Created by PhpStorm.
 * User: turbo
 * Date: 8/27/14
 * Time: 2:36 PM
 */

namespace TradeOwl\TradeBundle\Controller;

use Doctrine\ORM\Configuration;
use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\Security\Core\SecurityContextInterface;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\Form\Extension\HttpFoundation;
use Symfony\Component\HttpFoundation\Request;
use Doctrine\ORM\Query\Expr;
use TradeOwl\TradeBundle\Entity\PostItemRepository;
use Doctrine\ORM\QueryBuilder;

class SearchController extends Controller {

    public function searchAction(Request $request, QueryBuilder $qb=null)    {
        $user = $this->container->get('security.context')->getToken()->getUser();
        $mainSearch = $request->get('searchText');
        $userSearch = $request->get('userSearchText');
        $tagSearch = $request->get('tagSearchText');
        $geoSearch = $request->get('geoFilterID');
        //throw new Exception($mainSearch . ", ".$userSearch . ", " . $tagSearch . ", " . $geoSearch);
        $haveCriteria = false;
        $em = $this->getDoctrine()->getManager();
        $qb = $em->createQueryBuilder();
        $qb->addSelect('pi')
        ->from('TradeOwlTradeBundle:PostItem', 'pi')
            ->join('pi.user', 'piu', 'WITH', 'piu.id <> :userId');
        if(is_object($user) && $geoSearch != null && $geoSearch != ""){
            $qb->join('pi.geoFilter', 'pigf')
                ->join('pigf.polygons', 'pipoly')
                ->join('TradeOwlTradeBundle:GeoPolygon', 'upoly', 'WITH', 'OverlapsFunction(upoly.id, pipoly.id) = 0')
                ->join('upoly.filter', 'ugf', 'WITH', 'ugf.id = :geoSearch')
                ->setParameter('geoSearch', $geoSearch);
            $haveCriteria = true;
        }
        if ($mainSearch != null && trim($mainSearch) != "") {
            $qb->where('pi.title LIKE :mainSearch OR pi.body LIKE :mainSearch')
                ->setParameter('mainSearch', '%'.$mainSearch.'%');
            $haveCriteria = true;
        }
        if ($haveCriteria){
            $qb->andWhere('pi.isActive = 1');
        } else {
            $qb->where('pi.isActive = 1');
        }
        $haveCriteria = true;
        if (is_object($user) && $userSearch != null && trim($userSearch) != "") {
            if (!$haveCriteria) {
                $qb->where('piu.username LIKE :userSearch');
            } else {
                $qb->andWhere('piu.username LIKE :userSearch');
            }
            $qb->setParameter('userSearch', '%'.$userSearch.'%');
            $haveCriteria = true;
        }
        if (is_object($user) && $tagSearch != null && trim($tagSearch) != "") {
            if (!$haveCriteria) {
                $qb->where('');
            } else {
                $qb->andWhere('');
            }
            $haveCriteria = true;
        }
        $qb->setParameter('userId', -1);
        $qb->orderBy('pi.createDttm', 'DESC');
        $posts = $qb->getQuery()->getResult();
        if (!$posts) {
            $noPosts = array("response" => "No Posts Found.");
            return new jsonresponse($noPosts);
        }
        $postsfs = $this->postStrip($posts);
        return new jsonresponse($postsfs);
    }

    private function postStrip($posts){
        //TODO: Figure out where to put this utility
        $i=0;
        $postsfs=null;
        foreach($posts as $post) {
            $postID = $post->getId();
            $postTitle = $post->getTitle();
            $postBody = $post->getBody();
            $postDate = $post->getcreateDttm();
            $postsfs[$i]["id"] = $postID;
            $postsfs[$i]["title"] = $postTitle;
            $postsfs[$i]["body"] = $postBody;
            $postsfs[$i]["postDate"] = $postDate;
            foreach ($post->getPics() as $pic){
                $postsfs[$i]["img"] = $pic->getThumbSourceWebPath();
                break;
            }
            $i++;
        }
        return $postsfs;
    }
}