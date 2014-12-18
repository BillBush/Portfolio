<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 10/3/14
 * Time: 10:32 AM
 */

namespace TradeOwl\TradeBundle\Entity\Functions;

use Doctrine\ORM\Query\AST\Functions\FunctionNode;
use Doctrine\ORM\Query\Lexer;
use Doctrine\ORM\Query\Parser;
use Doctrine\ORM\Query\SqlWalker;

class GeoPolygonStr extends FunctionNode {
    private $arg;

    public function getSql(SqlWalker $sqlWalker){
        return 'GeomFromText(' . $this->arg->dispatch($sqlWalker) . ")";
    }

    public function parse(Parser $parser){
    }
} 