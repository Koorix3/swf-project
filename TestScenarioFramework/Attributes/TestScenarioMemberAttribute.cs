using System;
using System.Collections.Generic;
using System.Text;

namespace TestScenarioFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TestScenarioMemberAttribute : Attribute
    {
        public int Multiplicity;
        public object Min;
        public object Max;
        public bool Exclude;
    }
}
