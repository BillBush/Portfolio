<?php
namespace TradeOwl\TradeBundle\Entity\PostItemRepository;

use Doctrine\ORM\EntityRepository;
use Doctrine\ORM\Mapping as ORM;
use Symfony\Component\Form\Extension\HttpFoundation;
use Doctrine\ORM\Query\Expr;
use Symfony\Component\HttpKernel\Controller;
use Doctrine\ORM\QueryBuilder;

class PostItemRepository extends EntityRepository{

    public function getPostItems ($string, $repository) {

            return $this->getEntityManager()->createQuery('SELECT u FROM ApanaMainBundle:User u
                WHERE u.firstname LIKE :string OR u.lastname LIKE :string')
                ->setParameter('string','%'.$string.'%')
                ->getResult();
    }
}