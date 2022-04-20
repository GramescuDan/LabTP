using System;
using System.Collections;
using System.Net.Mime;

namespace NewProject.Models
{
    public class Token
    {
        public EnumCodes Code { get;}
        public int Line { get; }
        
        public dynamic Value;

        public Token(int line, EnumCodes code)
        {
            Code = code;
            Line = line;
        }
        
    }
}