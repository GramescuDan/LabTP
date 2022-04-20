using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;

namespace NewProject
{
    class Program
    {
        static public void printFile(List<String> lines)
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

            TokenHandler tkHandler = new TokenHandler(lines);
            tkHandler.StartCompile();
        }
        
    }
}