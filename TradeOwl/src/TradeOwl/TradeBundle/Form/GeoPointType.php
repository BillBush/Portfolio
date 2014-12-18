<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 9/15/14
 * Time: 1:55 PM
 */

namespace TradeOwl\TradeBundle\Form;

use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolverInterface;


class GeoPointType extends AbstractType {
    public function buildForm(FormBuilderInterface $builder, array $options){
        $builder->add('lng');
        $builder->add('lat');
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(
            array(
                'data_class' => 'TradeOwl\TradeBundle\Entity\GeoPoint',
            )
        );
    }

    public function getName()
    {
        return 'geo_point';
    }
} 