<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 9/19/14
 * Time: 12:25 PM
 */

namespace TradeOwl\TradeBundle\Form;

use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolverInterface;

class GeoFilterType extends AbstractType
{
    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder->add('name');
        $builder->add('polygons', 'collection', array('by_reference' => false, 'type' => new GeoPolygonType(), 'allow_add' => true,
            'allow_delete' => true, 'delete_empty' => true, 'prototype' => true, 'prototype_name' => '_polygon_name_placeholder_',
            'cascade_validation' => true, 'required' => false, 'label' => false));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(array(
            'data_class' => 'TradeOwl\TradeBundle\Entity\GeoFilter'
        ));
    }

    public function getName()
    {
        return 'geo_filter';
    }
} 