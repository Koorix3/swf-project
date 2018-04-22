using System;
using System.Collections.Generic;
using System.Text;

namespace TestScenarioFramework
{
    /// <summary>
    /// This exception is thrown when attributes are used incorrectly on entity classes.
    /// </summary>
    public class TestScenarioException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TestScenarioException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        public TestScenarioException(string message) : base(message)
        {
        }
    }
}
