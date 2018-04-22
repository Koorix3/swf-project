using System;

namespace TestScenarioFramework.Attributes
{
    /// <summary>
    /// Specifies conditions for populating a testscenario entity field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TestScenarioMemberAttribute : Attribute
    {
        /// <summary>
        /// Specifies the number of entries created for list types.
        /// </summary>
        public int Multiplicity;

        /// <summary>
        /// Specifies the minimum value of a field.
        /// </summary>
        public object Min;

        /// <summary>
        /// Specifies the maximum value of a field.
        /// </summary>
        public object Max;

        /// <summary>
        /// Excludes the field from random data generation.
        /// </summary>
        public bool Exclude;
    }
}
