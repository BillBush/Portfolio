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


class SearchQueryType extends AbstractType {

    public function buildForm(FormBuilderInterface $builder, array $options)
    {
        $builder
            ->add('queryField', 'text')
            ->add('save', 'submit', array('label' => 'Create Post'))
            ->getForm();
    }

    public function getName()
    {
        return 'search';
    }
}