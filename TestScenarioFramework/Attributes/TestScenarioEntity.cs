using System;

namespace TestScenarioFramework.Attributes
{
    /// <summary>
    /// Specifies, that the type can be used as a testscenario entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TestScenarioEntity : Attribute
    {
    }
}
