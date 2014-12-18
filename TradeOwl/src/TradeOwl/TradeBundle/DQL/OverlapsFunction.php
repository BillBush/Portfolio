<?php
/**
 * Created by PhpStorm.
 * User: john
 * Date: 10/3/14
 * Time: 10:09 AM
 */

namespace TradeOwl\TradeBundle\DQL;

use Doctrine\ORM\Query\AST\Functions\FunctionNode;
use Doctrine\ORM\Query\Lexer;
use Doctrine\ORM\Query\Parser;
use Doctrine\ORM\Query\SqlWalker;

class OverlapsFunction extends FunctionNode{
    private $firstArg;
    private $secondArg;

    public function getSql(SqlWalker $sqlWalker){
        return 'fAreDisjoint('.
        $this->firstArg->dispatch($sqlWalker) .
        ', ' .
        $this->secondArg->dispatch($sqlWalker) .
        ') ';
/*      return 'ST_Disjoint(' .
        $this->firstArg->dispatch() .
        ', '.
        $this->secondArg->dispathc() .
        ')';
*/
    }

    public function parse(Parser $parser){
        $parser->match(Lexer::T_IDENTIFIER);
        $parser->match(Lexer::T_OPEN_PARENTHESIS);
        $this->firstArg=$parser->ArithmeticPrimary();
        $parser->match(Lexer::T_COMMA);
        $this->secondArg=$parser->ArithmeticPrimary();
        $parser->match(Lexer::T_CLOSE_PARENTHESIS);
    }
} 