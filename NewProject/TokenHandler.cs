using System;
using System.Collections.Generic;
using NewProject.Models;

namespace NewProject
{
    public class TokenHandler
    {
        private  int _line;
        private readonly List<Token> _tokenList;
        private static String[] _lines;
        private static int _crtChar = -1;
        
        
        public TokenHandler(string[] lines)
        {
            _lines = lines;
            _tokenList = new List<Token>();
        }

        public void AddToken( int line,EnumCodes code)
        {
            Token token = new Token(line,code);
            _tokenList.Add(token);
        }

        public void PrintTokenList()
        {
            foreach (Token token in _tokenList)
            {
                Console.WriteLine(token._code);
            }
        }

        public void ErrMessage(String message)
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
            if (_lines[_line].Length == _crtChar)
            {
                if (skip)
                {
                    NextLine();
                }
                return true;
        }
        return false;
    }

        private bool IsEndFile()
        {
            if (_lines.Length == _line)
            {
                return true;
            }

            return false;
        }

        public int GetNextToken()
        {
            int state = 0;
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
                            state = 4;
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
                                ErrMessage("Undifined Token");
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
                                ErrMessage("Undifined Token");
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
                                state = 12;
                            }
                            state = 11;
                            
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
                        _crtChar++;
                        if (_lines[_line][_crtChar].CompareTo('/') == 0)
                        {
                            state = 0;
                            NextLine();
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
                        ErrMessage("Undifined Token");
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
                        ErrMessage("Undifined Token");
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
                        state = 12;
                        break;

                    case 12:
                        _crtChar--;
                        AddToken(_line,EnumCodes.NOT);
                        return (int) EnumCodes.NOT;
                    
                    case 13:
                        AddToken(_line,EnumCodes.NOTEQ);
                        return (int) EnumCodes.NOTEQ;
                }
                


            }

            return -1;
            }
        

        public void StartCompile()
        {
            /*while (GetNextToken()!=(int) EnumCodes.END)
            {
            
            }*/

            GetNextToken();
            GetNextToken();
            GetNextToken();
            
            GetNextToken();
            GetNextToken();
            
            
            PrintTokenList();
        }
    }
}