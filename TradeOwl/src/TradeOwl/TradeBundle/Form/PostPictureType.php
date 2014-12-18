<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 8/24/14
 * Time: 7:52 AM
 */

namespace TradeOwl\TradeBundle\Form;


use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolverInterface;

class PostPictureType extends AbstractType {

    public function buildForm(FormBuilderInterface $builder, array $options){
        $builder->add('file','file',array('label' => false, 'required'=>false, 'multiple'=>false));//TODO:allow user to select multiple files
        $builder->add('remove', 'button',array('label'=>false,'disabled'=>true,'attr' => array('class' => 'removePicButtonDisabled')));
    }

    public function setDefaultOptions(OptionsResolverInterface $resolver)
    {
        $resolver->setDefaults(
            array(
                'data_class' => 'TradeOwl\TradeBundle\Entity\PostPicture',
            )
        );
    }

    public function getName()
    {
        return 'post_pic';
    }
}