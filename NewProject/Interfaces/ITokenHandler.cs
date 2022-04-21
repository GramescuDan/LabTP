using System.Collections.Generic;
using NewProject.Models;

namespace NewProject.Interfaces
{
    public interface ITokenHandler
    {
        public List<Token> GetTokens();
    }
}