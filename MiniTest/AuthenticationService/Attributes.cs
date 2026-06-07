using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeEachAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class AfterEachAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class PriorityAttribute : Attribute 
    { 
        public int Priority { get; }
        public PriorityAttribute(int priority) 
        {
            Priority = priority;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class  DataRowAttribute : Attribute 
    { 
        public object?[] Data { get; }
        public string? Description { get; set; }

        public DataRowAttribute(params object?[] data)
        {
            Data = data;
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute 
    { 
        public string Description { get;}
        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
