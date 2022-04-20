using System;
using System.Collections.Generic;
using NewProject.Models;

namespace NewProject
{
    public class TokenHandler
    {
        private  int _line;
        private readonly List<Token> _tokenList;
        private static string[] _lines;
        private static int _crtChar = -1;
        
        
        public TokenHandler(string[] lines)
        {
            _lines = lines;
            _tokenList = new List<Token>();
        }

        private void AddToken( int line,EnumCodes code)
        {
            var token = new Token(line,code);
            _tokenList.Add(token);
        }

        private void AddToken( int line,EnumCodes code, dynamic value)
        {
            var token = new Token(line,code)
            {
                Value = value
            };
            _tokenList.Add(token);
        }

        private void PrintTokenList()
        {
            foreach (var token in _tokenList)
            {
                Console.WriteLine(token.Code);
            }
        }

        private void ErrMessage(String message)
        {
            Console.Error.WriteLine($"At line {_line}: {message}.Please Resolve!");
            Environment.Exit(-1);
        }

        private void NextLine()
        {
            _line++;
            _crtChar = 0;
        }
        
        private bool IsEndLine(bool skip)
        {
            if (_lines[_line].Length != _crtChar) return false;
            if (skip)
            {
                NextLine();
            }
            return true;
        }

        private int StringToInt()
        {
            var value = 0;
            while (true)
            {
                if (IsEndLine(false))
                {
                    return value/10 ;
                }

                if (Char.IsDigit(_lines[_line][_crtChar]))
                {
                    value += Convert.ToInt32(_lines[_line][_crtChar]);
                    value -= '0';
                    value *= 10; 
                    _crtChar++; 
                }
                else
                {
                    return value / 10;
                }
            }
        }
        private bool IsEndFile()
        {
            return _lines.Length == _line;
        }

        private int GetNextToken()
        {
            var state = 0;
            _crtChar += 1;
            while (true)
            {

                if (IsEndFile())
                {
                    AddToken(_line, EnumCodes.END);
                    return (int) EnumCodes.END;
                }

                if (IsEndLine(true))
                {
                    continue;
                }


                int value = 0;
                switch (state)
                {
                    case 0:
                        if (_lines[_line][_crtChar].CompareTo(' ') == 0 || _lines[_line][_crtChar].CompareTo('\t') == 0|| _lines[_line][_crtChar].CompareTo('\r') == 0)
                        {
                            _crtChar++;
                        }
                        if (_lines[_line][_crtChar].CompareTo('+') == 0)
                        {
                            state = 1;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('-') == 0)
                        {
                            state = 2;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('*') == 0)
                        {
                            state = 3;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('/') == 0)
                        {
                            _crtChar++;
                            if (IsEndLine(false))
                            {
                                _crtChar--;
                                state = 5;
                            }
                            else
                            {
                                state = 4;  
                            }
                        }
                        else if (_lines[_line][_crtChar].CompareTo('.') == 0)
                        {
                            state = 6;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('&') == 0)
                        { 
                            _crtChar++;
                            if (IsEndLine(true))
                            {
                                ErrMessage("Undefined Token");
                            }
                            else
                            {
                                state = 7;
                            }
                        }
                        else if (_lines[_line][_crtChar].CompareTo('|') == 0)
                        {
                            _crtChar++;
                            if (IsEndLine(true))
                            {
                                ErrMessage("Undefined Token");
                            }
                            else
                            {
                                state = 9;
                            }
                        }
                        else if (_lines[_line][_crtChar].CompareTo('!') == 0)
                        {
                            _crtChar++;
                            if (IsEndLine(false))
                            {
                                _crtChar--;
                                state = 12;
                            }
                            else
                            {
                                state = 11;  
                            }

                        }
                        else if (_lines[_line][_crtChar].CompareTo('=') == 0)
                        {
                            _crtChar++;
                            if (IsEndLine(false))
                            {
                                _crtChar--;
                                state = 15;
                            }
                            else
                            {
                                state = 14;  
                            }

                        }
                        else if (_lines[_line][_crtChar].CompareTo('<') == 0)
                        {
                            _crtChar++;
                            if (IsEndLine(false))
                            {
                                _crtChar--;
                                state = 18;
                            }
                            else
                            {
                                state = 17;  
                            }

                        }
                        else if (_lines[_line][_crtChar].CompareTo('>') == 0)
                        {
                            _crtChar++;
                            if (IsEndLine(false))
                            {
                                _crtChar--;
                                state = 21;
                            }
                            else
                            {
                                state = 20;  
                            }

                        }
                        else if (_lines[_line][_crtChar].CompareTo(',') == 0)
                        {
                            state = 23;
                        }
                        else if (_lines[_line][_crtChar].CompareTo(';') == 0)
                        {
                            state = 24;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('(') == 0)
                        {
                            state = 25;
                        }
                        else if (_lines[_line][_crtChar].CompareTo(')') == 0)
                        {
                            state = 26;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('[') == 0)
                        {
                            state = 27;
                        }
                        else if (_lines[_line][_crtChar].CompareTo(']') == 0)
                        {
                            state = 28;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('{') == 0)
                        {
                            state = 29;
                        }
                        else if (_lines[_line][_crtChar].CompareTo('}') == 0)
                        {
                            state = 30;
                        }
                        else if (Char.IsDigit(_lines[_line][_crtChar]))
                        {
                            state = 31;
                        }
                        break;
                    
                    case 1:
                    {
                        AddToken(_line,EnumCodes.ADD);
                        return (int) EnumCodes.ADD;
                    }
                    case 2 :
                        AddToken(_line,EnumCodes.SUB);
                        return (int) EnumCodes.SUB;
                    
                    case 3 :
                        AddToken(_line,EnumCodes.MUL);
                        return (int) EnumCodes.MUL;
                    
                    case 4:
                        if (_lines[_line][_crtChar].CompareTo('/') == 0)
                        {
                            NextLine();
                            state = 0;
                            break;
                        }

                        _crtChar--;
                        state = 5;
                        break;

                    case 5:
                            AddToken(_line,EnumCodes.DIV);
                            return (int) EnumCodes.DIV;
                    
                    case 6:
                        AddToken(_line,EnumCodes.DOT);
                        return (int) EnumCodes.DOT;
                    
                    case 7:
                        if (_lines[_line][_crtChar].CompareTo('&') == 0)
                        {
                            state = 8;
                            break;
                        }
                        ErrMessage("Undefined Token");
                        break;
                    
                    case 8:
                        AddToken(_line,EnumCodes.AND);
                        return (int) EnumCodes.AND;
                    
                    case 9:
                        if (_lines[_line][_crtChar].CompareTo('|') == 0)
                        {
                            state = 10;
                            break;
                        }
                        ErrMessage("Undefined Token");
                        break;
                    
                    case 10:
                        AddToken(_line,EnumCodes.OR);
                        return (int) EnumCodes.OR;
                    
                    case 11:
                        if (_lines[_line][_crtChar].CompareTo('=') == 0)
                        {
                            state = 13;
                            break;
                        }
                        _crtChar--;
                        state = 12;
                        break;

                    case 12:
                        AddToken(_line,EnumCodes.NOT);
                        return (int) EnumCodes.NOT;
                    
                    case 13:
                        AddToken(_line,EnumCodes.NOTEQ);
                        return (int) EnumCodes.NOTEQ;
                    
                    case 14:
                        if (_lines[_line][_crtChar].CompareTo('=') == 0)
                        {
                            state = 16;
                            break;
                        }
                        _crtChar--;
                        state = 15;
                        break;

                    case 15:
                        AddToken(_line,EnumCodes.ASSIGN);
                        return (int) EnumCodes.ASSIGN;
                    
                    case 16:
                        AddToken(_line,EnumCodes.EQUAL);
                        return (int) EnumCodes.EQUAL;
                    
                    case 17:
                        if (_lines[_line][_crtChar].CompareTo('=') == 0)
                        {
                            state = 19;
                            break;
                        }
                        _crtChar--;
                        state = 18;
                        break;

                    case 18:
                        AddToken(_line,EnumCodes.LESS);
                        return (int) EnumCodes.LESS;
                    
                    case 19:
                        AddToken(_line,EnumCodes.LESSEQ);
                        return (int) EnumCodes.LESSEQ;
                    
                    case 20:
                        if (_lines[_line][_crtChar].CompareTo('=') == 0)
                        {
                            state = 22;
                            break;
                        }
                        _crtChar--;
                        state = 21;
                        break;

                    case 21:
                        AddToken(_line,EnumCodes.GREATER);
                        return (int) EnumCodes.GREATER;
                    
                    case 22:
                        AddToken(_line,EnumCodes.GREATEREQ);
                        return (int) EnumCodes.GREATEREQ;
                    
                    case 23:
                        AddToken(_line,EnumCodes.COMMA);
                        return (int) EnumCodes.COMMA;
                    
                    case 24:
                        AddToken(_line,EnumCodes.SEMICOLON);
                        return (int) EnumCodes.SEMICOLON;
                    
                    case 25:
                        AddToken(_line,EnumCodes.LPAR);
                        return (int) EnumCodes.LPAR;
                    
                    case 26:
                        AddToken(_line,EnumCodes.RPAR);
                        return (int) EnumCodes.RPAR;
                    
                    case 27:
                        AddToken(_line,EnumCodes.LBREAK);
                        return (int) EnumCodes.LBREAK;
                    
                    case 28:
                        AddToken(_line,EnumCodes.RBREAK);
                        return (int) EnumCodes.RBREAK;
                    
                    case 29:
                        AddToken(_line,EnumCodes.LACC);
                        return (int)EnumCodes.LACC;
                    
                    case 30:
                        AddToken(_line,EnumCodes.RACC);
                        return (int) EnumCodes.RACC;
                    case 31: 
                        value = StringToInt();
                        Console.WriteLine(value);
                        _crtChar--;
                        state = 32;
                        break;
                    case 32:
                        AddToken(_line,EnumCodes.CT_INT,value);
                        return (int) EnumCodes.CT_INT;

                }
                


            }

            return -1;
            }
        

        public void StartCompile()
        {
            /*while (GetNextToken()!=(int) EnumCodes.END)
            {
            
            }*/

           // Console.WriteLine(StringToInt());
            
            //GetNextToken();
            GetNextToken();
            GetNextToken();
            GetNextToken();
            GetNextToken();
            GetNextToken();
            GetNextToken();


            PrintTokenList();
        }
    }
}