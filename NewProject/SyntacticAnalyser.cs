using System;
using System.Collections.Generic;
using NewProject.Interfaces;

namespace NewProject.Models
{
    public class SyntacticAnalyser
    {
        private readonly List<Token> _tokenList;

        public SyntacticAnalyser(ITokenHandler tokenHandler)
        {
            _tokenList = tokenHandler.GetTokens();
        }

        public void PrintTokens()
        {
            foreach (var token in _tokenList)
            {
                Console.WriteLine(token.Code);
            }
        }
    }
}