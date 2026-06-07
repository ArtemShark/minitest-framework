using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTestRunner
{
    public class TestResult
    {
        public int Total {  get; }
        public int Passed { get; }
        public int Failed { get; }

        public TestResult(int total, int passed, int failed)
        {
            Total = total;
            Passed = passed;
            Failed = failed;
        }
    }
}
