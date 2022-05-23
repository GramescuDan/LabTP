using System;
using System.Collections.Generic;

namespace NewProject
{
    class Program
    {
        static public void PrintFile(List<String> lines)
        {
            foreach (String line in lines)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("\n");
        }
        
        
        static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines("../../../Test_code");
            SyntacticAnalyser syAnalyser = new SyntacticAnalyser(new TokenHandler(lines));
            syAnalyser.PrintTokens();
        }
        
    }
}