<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 8/24/14
 * Time: 6:52 AM
 */

namespace TradeOwl\TradeBundle\Form;

use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolverInterface;
use TradeOwl\TradeBundle\Entity\User;
use Doctrine\ORM\EntityRepository;

class PostItemType extends AbstractType {

    protected $user;

    function __construct(User $user = null){
        $this->user = $user;
    }

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('title');
        $builder->add('body', 'textarea');
        if(!is_null($this->user)) {
            $builder->add('geoFilter', 'entity',
                array('class'=>'TradeOwl\TradeBundle\Entity\GeoFilter',
                    'property'=>'name', 'mapped'=>true,
                    'empty_value'=>'Choose an option', 'required'=>true,
                    'query_builder' => function (EntityRepository $er) {
                        return $er->createQueryBuilder('gf')
                            ->join('gf.user', 'u', 'WITH', 'u.id = ?1')
                            ->setParameter(1, $this->user->getId());
                    }));
        }
        $builder->add('pics', 'collection', array('by_reference'=>false,
            'type'=>new PostPictureType(), 'allow_add'=>true, 'allow_delete'=>true,
            'delete_empty'=>true, 'prototype'=>true,
            'prototype_name'=>'_pic_name_placeholder_', 'cascade_validation'=>true,
            'label'=>'Add pictures (limit 5)', 'label_attr'=>array('name'=>'picsLabel'),
            'required'=>false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\PostItem'
        ));
    }

    public function getName()
    {
        return 'post';
    }
}