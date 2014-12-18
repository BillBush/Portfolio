<?php
/**
 * Created by PhpStorm.
 * User: bill
 * Date: 11/16/14
 * Time: 10:37 AM
 */

namespace TradeOwl\TradeBundle\Form;

use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolverInterface;
use TradeOwl\TradeBundle\Form\TradeOfferType;

class TradeOfferChainType extends AbstractType {

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('offers', 'collection', array('by_reference' => false,'type' => new TradeOfferType(),
            'allow_add'=>false, 'allow_delete'=>false, 'delete_empty'=>true, 'prototype'=>true,
            'prototype_name'=>'_offers_name_placeholder_', 'cascade_validation'=>true, 'label'=>'Offer History',
            'label_attr'=>array('name'=>'offersLabel'),'required'=>false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\TradeOfferChain'
        ));
    }

    public function getName()
    {
        return 'chain';
    }
} 