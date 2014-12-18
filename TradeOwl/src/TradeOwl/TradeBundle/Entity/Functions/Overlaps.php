<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 10/3/14
 * Time: 10:09 AM
 */

namespace TradeOwl\TradeBundle\Entity\Functions;

use Doctrine\ORM\Query\AST\Functions\FunctionNode;
use Doctrine\ORM\Query\Lexer;
use Doctrine\ORM\Query\Parser;
use Doctrine\ORM\Query\SqlWalker;

class Overlaps extends FunctionNode{
    private $firstArg;
    private $secondArg;

    public function getSql(SqlWalker $sqlWalker){
        return 'ST_Disjoint(' .
        $this->firstArg->dispatch() .
        $this->secondArg->dispathc() .
        ')';
    }

    public function parse(Parser $parser){
    }
} 