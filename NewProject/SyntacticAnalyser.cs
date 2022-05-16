using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
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
            _iterator = 0;
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
                ErrMessage("Missing name for structure");
            }
            
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
        
        private bool VarDefine()
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
            }

            _iterator = ins;
            return false;
        }

        private bool StructDefine()
        {
            int ins=_iterator;
            if (Consume(EnumCodes.STRUCT))
            {
                if (Consume(EnumCodes.ID))
                {
                    if (Consume(EnumCodes.LACC))
                    {
                        while (VarDefine()) { }

                        if (Consume(EnumCodes.RACC))
                        {
                            if (Consume(EnumCodes.SEMICOLON))
                            {
                                return true;
                            }
                            ErrMessage("Missing ;");
                        }
                        ErrMessage("Missing }");
                    }
                    ErrMessage("Missing {");
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
                    if (ArrayDeclaration()) ;
                    return true;
                }
                ErrMessage("Missing name for the parameter");
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
                    if (ArrayDeclaration()) ;
                    if (Consume(EnumCodes.RPAR))
                    {
                        if (exprCast())
                        {
                            return true;
                        }
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
                        while (Consume(EnumCodes.COMMA) && expr()) { }
                    }
                    
                    if(Consume(EnumCodes.RPAR)) { }
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
                ErrMessage("Missing == after expression");
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
                while( VarDefine() || stm()){}

                if (Consume(EnumCodes.RACC))
                {
                    return true;
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
                                    if(stm()){}
                                }
                                
                                return true;
                            }
                        }
                    }
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
                        }
                    }
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
                            }
                        }
                    }
                }
            }

            if (Consume(EnumCodes.BREAK))
            {
                if (Consume(EnumCodes.SEMICOLON))
                {
                    return true;
                }
            }

            if (Consume(EnumCodes.RETURN))
            {
                if(expr()){}

                if (Consume(EnumCodes.SEMICOLON))
                {
                    return true;
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
            while (true)
            {
                if (VarDefine())
                {
                    continue;
                }
                else if (funDef())
                {
                    continue;
                }
                else if (VarDefine())
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (Consume(EnumCodes.END))
            {
                return true;
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
                            while (Consume(EnumCodes.COMMA) && funParam()){}

                            if (Consume(EnumCodes.RPAR))
                            {
                                if (stmCompound())
                                {
                                    return true;
                                }
                            }
                        }
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
                            while (Consume(EnumCodes.COMMA) && funParam()){}

                            if (Consume(EnumCodes.RPAR))
                            {
                                if (stmCompound())
                                {
                                    return true;
                                }
                            }
                        }
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
                ErrMessage("Field name missing  after .");
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
                    ErrMessage("missing add expression after mul expression");
                }
                ErrMessage("Missing mul expression after +");
            }else if (Consume(EnumCodes.SUB))
            {
                if (exprMul())
                {
                    if (exprAddPrim())
                    {
                        return true;
                    }
                }
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
            }else if (Consume(EnumCodes.LESSEQ))
            {
                if (exprAdd())
                {
                    if (exprRelPrim())
                    {
                        return true;
                    }
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
            }
            
            return true;
        }
        private bool exprEqPrim()
        {
            
            if (Consume(EnumCodes.EQUAL))
            {
                if (exprEq())
                {
                    if (exprEqPrim())
                    {
                        return true;
                    }
                }
            }
            
            if (Consume(EnumCodes.NOTEQ))
            {
                if (exprEq())
                {
                    if (exprEqPrim())
                    {
                        return true;
                    }
                }
            }
            
            return true;
        }
        
        
        
        public void PrintTokens()
        {
            
            Console.WriteLine(unit());
        }
    }
}