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
            
            
            
            _iterator = ins;
            return false; 
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
                }
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
            foreach (Token token in _tokenList)
            {
                Console.Write(token.Line+ ":");
                Console.Write(token.Code);
                if (token.Code == EnumCodes.ID || token.Code == EnumCodes.CT_INT || token.Code == EnumCodes.CT_REAL || token.Code == EnumCodes.CT_CHAR || token.Code == EnumCodes.CT_STRING)
                { 
                    Console.Write(" ->" + token.Value);  
                }
                Console.WriteLine();
            }
        }
    }
}