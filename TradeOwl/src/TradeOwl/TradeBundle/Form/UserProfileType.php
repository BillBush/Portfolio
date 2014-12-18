<?php
/**
 * Created by PhpStorm.
 * User: bill
 * Date: 11/15/14
 * Time: 10:49 AM
 */

namespace TradeOwl\TradeBundle\Form;

use FOS\UserBundle\Form\Type\ProfileFormType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolverInterface;
use TradeOwl\TradeBundle\Entity\User;
use TradeOwl\TradeBundle\Form\PostItemType;
use TradeOwl\TradeBundle\Form\TradeOfferChainType;

class UserProfileType extends ProfileFormType {

    public function __construct()
    {
        parent::__construct('TradeOwl\\TradeBundle\\Entity\\User');
    }

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        parent::buildForm($builder, $options);
        $builder->add('pic', 'collection', array('by_reference' => false,
            'type' => new PostPictureType(), 'allow_add'=>true,'allow_delete'=>true,
            'delete_empty'=>true, 'prototype'=>true,
            'prototype_name'=>'_pic_name_placeholder_', 'cascade_validation'=>true,
            'label'=>'Change Picture', 'label_attr'=>array('name'=>'userPicsLabel'),
            'required'=>false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\User',
            'intention'  => 'profile',
        ));
    }

    public function getName()
    {
        return 'user';
    }
} 