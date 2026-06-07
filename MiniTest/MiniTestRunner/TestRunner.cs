using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace MiniTestRunner
{
    public class TestRunner
    {
        private int totalTests = 0;
        private int totalPassed = 0;
        private int totalFailed = 0;

        public void ExecuteAssembly(string path)
        {
            try
            {
                Console.WriteLine($"Loading assembly: {path}");
                AssemblyLoadContext context = new AssemblyLoadContext("TestContext", isCollectible: true);
                using FileStream assemblyStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                Assembly assembly = context.LoadFromStream(assemblyStream);

                TestResult results = RunTests(assembly);
                totalTests += results.Total;
                totalPassed += results.Passed;
                totalFailed += results.Failed;

                context.Unload();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Not loading assembly {path}: {ex.Message}");
            }
        }

        private TestResult RunTests(Assembly assembly)
        {
            var testClasses = assembly.GetTypes().Where(a=>a.GetCustomAttribute<AuthenticationService.TestClassAttribute>() != null).ToList();

            if (!testClasses.Any()) 
            {
                Console.WriteLine($"No tests classes are in assembly {assembly.FullName}");
                return new TestResult(0,0,0);
            }

            int totalTests = 0, passed = 0, failed = 0;

            foreach (var testClass in testClasses)
            { 
                Console.WriteLine($"Running tests from class {testClass.FullName}...");
                TestResult results = ExecuteTestClass(testClass);
                totalTests += results.Total;
                passed += results.Passed;
                failed += results.Failed;

                Print(results.Total, results.Passed, results.Failed);
                Console.WriteLine("################################################################################");
            }

            return new TestResult(totalTests, passed, failed);
        }

        private TestResult ExecuteTestClass(Type testClass)
        {
            ConstructorInfo? constructor = testClass.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"No parameterless constructor: {testClass.Name}");
                Console.ResetColor();
                return new TestResult(0,0,0);
            }

            object? instance = Activator.CreateInstance(testClass);
            if (instance == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Can't create instance of class");
                Console.ResetColor();
                return new TestResult(0, 0, 0);
            }

            var beforeEach = Bind(testClass, typeof(AuthenticationService.BeforeEachAttribute));
            var afterEach = Bind(testClass, typeof(AuthenticationService.AfterEachAttribute));

            var testMethods = testClass.GetMethods().Where(a=>a.GetCustomAttribute<AuthenticationService.TestMethodAttribute>() != null)
                .OrderBy(a=>a.GetCustomAttribute<AuthenticationService.PriorityAttribute>()?.Priority ?? 0).ThenBy(a=>a.Name).ToList();

            int totalTests = 0, passed = 0, failed = 0;

            foreach (var testMethod in testMethods)
            { 
                string? description = testMethod.GetCustomAttribute<AuthenticationService.DescriptionAttribute>()?.Description;
                var dataRows = testMethod.GetCustomAttributes<AuthenticationService.DataRowAttribute>();

                if (dataRows.Any())
                {
                    foreach (var row in dataRows)
                    {
                        Console.Write($"{testMethod.Name}");
                        if (ExecuteTest(instance, beforeEach, afterEach, testMethod, row.Data))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($": PASSED");
                            Console.ResetColor();
                            passed++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($": FAILED");
                            Console.ResetColor();
                            failed++;
                        }
                        totalTests++;
                    }
                }
                else
                {
                    Console.Write($"{testMethod.Name}");
                    if (ExecuteTest(instance, beforeEach, afterEach, testMethod, null))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($": PASSED");
                        Console.ResetColor();
                        passed++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($": FAILED");
                        Console.ResetColor();
                        failed++;
                    }
                    totalTests++;
                }
            }
            return new TestResult(totalTests, passed, failed);
        }

        private Action<object>? Bind(Type testClass, Type attributeType)
        {
            var method = testClass.GetMethods().FirstOrDefault(a=>a.GetCustomAttribute(attributeType) != null);

            if (method != null)
                return instance => method.Invoke(instance, null);
            else 
                return null;
        }
        private bool ExecuteTest(object instance, Action<object>? beforeEach, Action<object>? afterEach, MethodInfo testMethod, object?[]? parameters)
        {
            try
            {
                beforeEach?.Invoke(instance);
                testMethod.Invoke(instance, parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  {ex.Message}");
                return false;
            }
            finally
            {
                afterEach?.Invoke(instance);
            }
        }

        public void PrintFinal()
        {
            Console.WriteLine("Summary of running tests:");
            Print(totalTests, totalPassed, totalFailed);
        }

        private void Print(int total, int passed, int failed)
        {
            Console.WriteLine("*******************************************");
            Console.WriteLine($"* Test passed: {passed} / {total}    *");
            Console.WriteLine($"* Failed:      {failed}         *");
            Console.WriteLine("*******************************************");
        }
    }
}
