<?php
/**
 * Created by PhpStorm.
 * User: bill
 * Date: 11/17/14
 * Time: 9:19 AM
 */

namespace TradeOwl\TradeBundle\Form;

    use Symfony\Component\Form\AbstractType;
    use Symfony\Component\Form\FormBuilderInterface;
    use Symfony\Component\OptionsResolver\OptionsResolverInterface;
    use TradeOwl\TradeBundle\Entity\TradeOffer;

class TradeOfferLastType extends AbstractType {

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('createDttm', 'datetime', array('format'=>'yy, MMMM d, h:m:s a',
            'widget'=>'single_text', 'empty_value'=>false, 'disabled'=>true));
        $builder->add('accept', 'button');
        $builder->add('posts', 'collection', array('by_reference'=>false,
            'type'=>new PostItemTitleType(null), 'allow_add'=>false, 'allow_delete'=>false,
            'prototype'=>false, 'label'=>'Items In Offer',
            'label_attr'=>array('name'=>'itemsOfferedLabel'),'required'=>false));
        $builder->add('userSendingAry', 'collection', array('by_reference' => false,
            'type' => new UserNameType(), 'allow_add'=>false, 'allow_delete'=>false,
            'delete_empty'=>false, 'prototype'=>false, 'label'=>'Sending User',
            'label_attr'=>array('name'=>'sendingUserLabel'), 'required'=>false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\TradeOffer'
        ));
    }

    public function getName()
    {
        return 'offer';
    }
}

class UserNameType extends AbstractType{

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('username', 'text', array('disabled'=>true, 'label'=>false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\User'
        ));
    }

    public function getName()
    {
        return 'userName';
    }
}

class PostItemTitleType extends AbstractType{

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('title', 'text', array('disabled'=>true, 'label'=>false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\PostItem'
        ));
    }

    public function getName()
    {
        return 'postItemTitle';
    }
} 