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

class GeoPolygonType extends AbstractType
{

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('points', 'collection', array('by_reference' => false, 'type' => new GeoPointType(), 'allow_add' => true,
            'allow_delete' => true, 'delete_empty' => true, 'prototype' => true, 'prototype_name' => '_point_name_placeholder_',
            'cascade_validation' => true, 'required' => false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\GeoPolygon'
        ));
    }

    public function getName()
    {
        return 'geo_polygon';
    }
} 