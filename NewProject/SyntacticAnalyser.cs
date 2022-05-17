using System;
using System.Collections.Generic;
using NewProject.Interfaces;

namespace NewProject.Models
{
    public class SyntacticAnalyser
    {
        private readonly List<Token> _tokenList;
        private int _iterator;


        public SyntacticAnalyser(ITokenHandler tokenHandler)
        {
            _tokenList = tokenHandler.GetTokens();
        }
        
        private void ErrMessage(String message)
        {
            Console.Error.WriteLine($"At line {_tokenList[_iterator].Line}: {message}.Please Resolve!");
            Environment.Exit(-1);
        }

        private bool Consume(EnumCodes code)
        {
            if (_tokenList[_iterator].Code.Equals(code))
            {
                _iterator++;
                return true;
            }
            return false;
        }

        private bool TypeBase()
        {
            int ins = _iterator;
            
            if (Consume(EnumCodes.INT))
            {
                return true;
            }
            if (Consume(EnumCodes.DOUBLE))
            {
                return true;
            }
            if (Consume(EnumCodes.CHAR))
            {
                return true;
            }
            if (Consume(EnumCodes.STRUCT))
            {
                if (Consume(EnumCodes.ID))
                {
                    return true;
                }
                else
                {
                    ErrMessage("Missing { for structure or this is not an name for the var"); 
                }
            }
            
            _iterator = ins;
            return false;
        }

        private bool ArrayDeclaration()
        {
            int ins = _iterator;
            if (Consume(EnumCodes.LBREAK))
            {
                if (Consume(EnumCodes.CT_INT))
                {
                    
                }

                if (Consume(EnumCodes.RBREAK))
                {
                    return true;
                }
                ErrMessage("Missing ]");
                
            }

            _iterator = ins;
            return false;
        }
        
        private bool VarDef()
        {
            int ins = _iterator;
            if (TypeBase())
            {
                if (Consume(EnumCodes.ID))
                {
                    if (ArrayDeclaration())
                    {
                        
                    }

                    if (Consume(EnumCodes.SEMICOLON))
                    {
                        return true;
                    }
                    ErrMessage("Missing ;");
                }
                else
                {
                    ErrMessage("Missing name after type");
                }
            }

            _iterator = ins;
            return false;
        }

        private bool StructDef()
        {
            int ins=_iterator;
            if (Consume(EnumCodes.STRUCT))
            {
                if (Consume(EnumCodes.ID))
                {
                    if (Consume(EnumCodes.LACC))
                    {
                        while (true)
                        {
                            if (VarDef())
                            {
                                
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (Consume(EnumCodes.RACC))
                        {
                            if (Consume(EnumCodes.SEMICOLON))
                            {
                                return true;
                            }
                            else
                            {
                                ErrMessage("Missing ;"); 
                            }
                        }
                        else
                        {
                            ErrMessage("Missing } after {");
                        }
                        
                    }
                    else
                    {
                        return false;
                    }
                }
                ErrMessage("Missing structure name");
            }
            
            _iterator = ins;
            return false;   
        }

        private bool funParam()
        {
            int ins = _iterator;
            if (TypeBase())
            {
                if (Consume(EnumCodes.ID))
                {
                    if (ArrayDeclaration()) {};
                    return true;
                }
                ErrMessage("Missing name for the variable");
            }

            _iterator = ins;
            return false;
        }

        private bool exprOr()
        {
            int ins = _iterator;
            if (exprAnd())
            {
                if (exprOrPrim())
                {
                    return true;
                } 
            }

            _iterator = ins;
            return false;
        }
        
        private bool exprAnd()
        {
            int ins = _iterator;

            if (exprEq())
            {
                if (exprAndPrim())
                {
                    return true;
                }
            }

            _iterator = ins;
            return false;
        }
        private bool exprEq()
        {
            int ins = _iterator;

            if (exprRel())
            {
                if (exprEqPrim())
                {
                    return true;
                }
            }
            
            _iterator = ins;
            return false;
        }
        private bool exprRel()
        {
            int ins = _iterator;

            if (exprAdd())
            {
                if (exprRelPrim())
                {
                    return true;
                }
            }
            
            _iterator = ins;
            return false;
        }
        
        private bool exprAdd()
        {
            int ins = _iterator;

            if (exprMul())
            {
                if (exprAddPrim())
                {
                    return true;
                }
            }
            
            _iterator = ins;
            return false;
        }
        private bool exprMul()
        {
            int ins = _iterator;

            if (exprCast())
            {
                if (exprMulPrim())
                {
                    return true;
                }  
            }
            
            _iterator = ins;
            return false;
        }
        private bool exprCast()
        {
            int ins = _iterator;

            if (Consume(EnumCodes.LPAR))
            {
                if (TypeBase())
                {
                    if (ArrayDeclaration()) {};
                    if (Consume(EnumCodes.RPAR))
                    {
                        if (exprCast())
                        {
                            return true;
                        }
                        else
                        {
                            ErrMessage("Invalid expression after cast");
                        }
                    }
                    else
                    {
                        ErrMessage("missing ) after (");
                    }
                }
            }else if (exprUnary())
            {
                return true;
            }
            
            _iterator = ins;
            return false;
        }
        private bool exprUnary()
        {
            int ins = _iterator;

            if (Consume(EnumCodes.SUB))
            {
                if (exprUnary())
                {
                    return true;
                }
                else
                {
                    ErrMessage("Missing Unary expression after -");
                }
            }

            if (Consume(EnumCodes.NOT))
            {
                if (exprUnary())
                {
                    return true;
                }
                else
                {
                    ErrMessage("Missing Unary expression after !");
                }
            }

            if (exprPostfix())
            {
                return true;
            }
            
            _iterator = ins;
            return false; 
        }

        private bool exprPostfix()
        {
            int ins = _iterator;

            if (exprPrimary())
            {
                if (exprPostfixPrim())
                {
                    return true;
                }
            }
            
            _iterator = ins;
            return false;
        }

        private bool exprPrimary()
        {
            int ins = _iterator;

            if (Consume(EnumCodes.ID))
            {
                if (Consume(EnumCodes.LPAR))
                {
                    if (expr())
                    {
                        while (true)
                        {
                            if (Consume(EnumCodes.COMMA))
                            {
                                if(expr()){}
                                else
                                {
                                    ErrMessage("Missing expression after ,");
                                }
                            }
                            else
                            {
                                break;
                            }
                            
                        }
                    }

                    if (Consume(EnumCodes.RPAR)) { }
                    else
                    {
                        ErrMessage("Missing ) after (");
                    }
                }
                return true;
            }

            if (Consume(EnumCodes.CT_INT))
            {
                return true;
            }
            if (Consume(EnumCodes.CT_REAL))
            {
                return true;
            }
            if (Consume(EnumCodes.CT_CHAR))
            {
                return true;
            }
            if (Consume(EnumCodes.CT_STRING))
            {
                return true;
            }

            if (Consume(EnumCodes.LPAR))
            {
                if (expr())
                {
                    if (Consume(EnumCodes.RPAR))
                    {
                        return true;
                    }
                    else
                    {
                        ErrMessage("Missing ) after (");
                    }
                }
                else
                {
                    _iterator = ins;
                    return false;
                }
            }
            
            _iterator = ins;
            return false;
        }

        private bool expr()
        {
            int ins = _iterator;
            if (exprAssign())
            {
                return true;
            }

            _iterator = ins;
            return false;
        }

        private bool exprAssign()
        {
            int ins = _iterator;
            if (exprUnary())
            {
                if (Consume(EnumCodes.ASSIGN))
                {
                    if (exprAssign())
                    {
                        return true;
                    }
                    ErrMessage("Missing Assign expression after assign");
                }
            }

            if (exprOr())
            {
                return true;
            }

            _iterator = ins;
            return false;
        }

        private bool stmCompound()
        {
            int ins = _iterator;

            if (Consume(EnumCodes.LACC))
            {
                while (true)
                {
                    if(VarDef()){}
                    else if (stm()) {}
                    else
                    {
                        break;
                    }
                }

                if (Consume(EnumCodes.RACC))
                {
                    return true;
                }
                else
                {
                    ErrMessage("Missing } after {");
                }
            }
            
            _iterator = ins;
            return false;
        }

        private bool stm()
        {
            int ins = _iterator;
            if (stmCompound())
            {
                return true;
            }

            if (Consume(EnumCodes.IF))
            {
                if (Consume(EnumCodes.LPAR))
                {
                    if (expr())
                    {
                        if (Consume(EnumCodes.RPAR))
                        {
                            if (stm())
                            {
                                if (Consume(EnumCodes.ELSE))
                                {
                                    if (stm())
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        ErrMessage("missing statement after else");
                                    }
                                }
                                
                                return true;
                            }
                            else
                            {
                                ErrMessage("Missing statement after if");
                            }
                        }
                        else
                        {
                            ErrMessage("Missing ) after (");
                        }
                    }
                    else
                    {
                        ErrMessage("Missing expression after (");
                    }
                }
                else
                {
                    ErrMessage("Missing ( after if");
                }
            }

            if (Consume(EnumCodes.WHILE))
            {
                if (Consume(EnumCodes.LPAR))
                {
                    if (expr())
                    {
                        if (Consume(EnumCodes.RPAR))
                        {
                            if (stm())
                            {
                                return true;
                            }
                            else
                            {
                                ErrMessage("Missing statement in while");
                            }
                        }else
                        {
                            ErrMessage("Missing ) after (");
                        }
                    }
                    else
                    {
                        ErrMessage("Missing expression after (");
                    }
                }
                else
                {
                    ErrMessage("Missing ( after while statement");
                }
            }

            if (Consume(EnumCodes.FOR))
            {
                if (Consume(EnumCodes.LPAR))
                {
                    if(expr()){}

                    if (Consume(EnumCodes.SEMICOLON))
                    {
                        if(expr()){}
                        
                        if (Consume(EnumCodes.SEMICOLON))
                        {
                            if(expr()){}

                            if (Consume(EnumCodes.RPAR))
                            {
                                if (stm())
                                {
                                    return true;
                                }
                                else
                                {
                                    ErrMessage("Missing statement after for");
                                }
                            }
                            else
                            {
                                ErrMessage("Missing ) after (");
                            }
                        }
                        else
                        {
                            ErrMessage("Missing 2nd ; in for statement");
                        }
                    }
                    else
                    {
                        ErrMessage("Missing 1st ; in for statement");
                    }
                }
                else
                {
                    ErrMessage("Missing ( after for");
                }
            }

            if (Consume(EnumCodes.BREAK))
            {
                if (Consume(EnumCodes.SEMICOLON))
                {
                    return true;
                }
                else
                {
                    ErrMessage("Missing ; after break");
                }
            }

            if (Consume(EnumCodes.RETURN))
            {
                if(expr()){}

                if (Consume(EnumCodes.SEMICOLON))
                {
                    return true;
                }
                else
                {
                    ErrMessage("Missing ; after return");
                }
            }
            
            if(expr()){}

            if (Consume(EnumCodes.SEMICOLON))
            {
                return true;
            }
                
            _iterator = ins;
            return false;
        }

        private bool unit()
        {
            for(;;)
            {
                if (StructDef()) { }
                else if(funDef()){ }
                else if(VarDef()){ }
                else
                {
                    break;
                }
            }

            if (Consume(EnumCodes.END))
            {
                return true;
            }
            else
            {
                Console.WriteLine(_iterator);
                ErrMessage("Missing end token or something happened");
            }

            return false;
        }

        private bool funDef()
        {
            int ins = _iterator;
            if (TypeBase())
            {
                if (Consume(EnumCodes.ID))
                {
                    if (Consume(EnumCodes.LPAR))
                    {
                        if (funParam())
                        {
                            while (true)
                            {
                                if (Consume(EnumCodes.COMMA))
                                {
                                    if (funParam())
                                    {
                                        
                                    }
                                    else
                                    {
                                        ErrMessage("No function parameter after ,");
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (Consume(EnumCodes.RPAR))
                            {
                                if (stmCompound())
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                ErrMessage("Missing ) after (");
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            
            else if (Consume(EnumCodes.VOID))
            {
                if (Consume(EnumCodes.ID))
                {
                    if (Consume(EnumCodes.LPAR))
                    {
                        if (funParam())
                        {
                            while (true)
                            {
                                if (Consume(EnumCodes.COMMA))
                                {
                                    if (funDef())
                                    {
                                        
                                    }
                                    else
                                    {
                                        ErrMessage("missing function def after ,");
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (Consume(EnumCodes.RPAR))
                            {
                                if (stmCompound())
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                ErrMessage("Missing ) after (");
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            _iterator = ins;
            return false;
        }


        private bool exprPostfixPrim()
        {
            if (Consume(EnumCodes.LBREAK))
            {
                if (expr())
                {
                    if (Consume(EnumCodes.RBREAK))
                    {
                        if (exprPostfixPrim())
                        {
                            return true;
                        }
                    }
                    ErrMessage("Missing ] after expression");
                }
                ErrMessage("Missing expression after [");
            }

            if (Consume(EnumCodes.DOT))
            {
                if (Consume(EnumCodes.ID))
                {
                    if (exprPostfixPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("Field name missing  after .");
                }
            }
            
            return true;
        }


        private bool exprMulPrim()
        {
            if (Consume(EnumCodes.MUL))
            {
                if (exprCast())
                {
                    if (exprMulPrim())
                    {
                        return true;
                    }
                    ErrMessage("Missing mul expression after cast");
                }
                ErrMessage("Missing cast after *");
            }
            
            if (Consume(EnumCodes.DIV))
            {
                if (exprCast())
                {
                    if (exprMulPrim())
                    {
                        return true;
                    }
                    ErrMessage("Missing mul expression after cast");
                }
                ErrMessage("Missing cast after /");
            }
            
            return true;
        }
        
        private bool exprAddPrim()
        {
            if (Consume(EnumCodes.ADD))
            {
                if (exprMul())
                {
                    if (exprAddPrim())
                    {
                        return true;
                    }
                }
                else {ErrMessage("Missing mul expression after +");}
            }
            if (Consume(EnumCodes.SUB))
            {
                if (exprMul())
                {
                    if (exprAddPrim())
                    {
                        return true;
                    }
                }
                else{ErrMessage("Invalid expression after -");}
            }
            
            return true;
        }
        
        private bool exprRelPrim()
        {
            if (Consume(EnumCodes.LESS))
            {
                if (exprAdd())
                {
                    if (exprRelPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("invalid expression after <");
                }
            }else if (Consume(EnumCodes.LESSEQ))
            {
                if (exprAdd())
                {
                    if (exprRelPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("invalid expression after <=");
                }
            }else if (Consume(EnumCodes.GREATER))
            {
                if (exprAdd())
                {
                    if (exprRelPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("invalid expression after >");
                }
            }else 
            if (Consume(EnumCodes.GREATEREQ))
            {
                if (exprAdd())
                {
                    if (exprRelPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("invalid expression after >=");
                }
            }
            return true;
        }
        
        private bool exprOrPrim()
        {
            if (Consume(EnumCodes.OR))
            {
                if (exprAnd())
                {
                    if (exprOrPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("invalid expression after ||");
                }
            }
            return true;
        }

        private bool exprAndPrim()
        {
            
            if (Consume(EnumCodes.AND))
            {
                if (exprEq())
                {
                    if (exprAndPrim())
                    {
                        return true;
                    }
                }
                else
                {
                ErrMessage("invalid expression after &&");    
                }
            }
            
            return true;
        }
        private bool exprEqPrim()
        {
            
            if (Consume(EnumCodes.EQUAL))
            {
                if (exprRel())
                {
                    if (exprEqPrim())
                    {
                        return true;
                    }
                }
                else
                {
                    ErrMessage("invalid expression after ==");
                }
            }
            
            if (Consume(EnumCodes.NOTEQ))
            {
                if (exprRel())
                {
                    if (exprEqPrim())
                    {
                        return true;
                    }
                } 
                else
                {
                    ErrMessage("invalid expression after !=");
                }
            }
            
            return true;
        }
        
        
        
        public void PrintTokens()
        {
            /*foreach (var token in _tokenList)
            {
              Console.WriteLine(token.Code);  
            }*/
            Console.WriteLine(unit());
        }
    }
}