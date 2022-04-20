using System;
using System.Collections;
using System.Net.Mime;

namespace NewProject.Models
{
    public class Token
    {
        public EnumCodes _code { get;}
        public int _line { get; }
        
        public dynamic value;

        public Token(int line, EnumCodes code)
        {
            _code = code;
            _line = line;
        }
        
    }
}