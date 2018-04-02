using System;
using System.Collections.Generic;
using System.Text;

namespace TestScenarioFramework
{
    public class TestScenarioException : Exception
    {
        public TestScenarioException(string message) : base(message)
        {
        }
    }
}
