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
        
        
        public void PrintTokens()
        {
            Console.WriteLine(StructDefine());
        }
    }
}