using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Loader;

namespace MiniTestRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No assemblies provided");
                return;
            }

            TestRunner testRunner = new TestRunner();
            foreach (string path in args)
            {
                testRunner.ExecuteAssembly(path);
            }

            testRunner.PrintFinal();
        }
    }
}
