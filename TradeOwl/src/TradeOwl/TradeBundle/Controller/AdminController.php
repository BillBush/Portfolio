<?php
/**
 * Created by PhpStorm.
 * User: turbo
 * Date: 8/26/14
 * Time: 2:42 PM
 */

namespace TradeOwl\TradeBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\Security\Core\SecurityContextInterface;



class AdminController extends Controller{

    public function viewAction () {




        return $this->render('TradeOwlTradeBundle:Admin:admin_view.html.twig');
    }



} 