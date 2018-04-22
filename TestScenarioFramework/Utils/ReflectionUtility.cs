using System;

namespace TestScenarioFramework.Utils
{
    internal class ReflectionUtility
    {
        /// <summary>
        /// Invokes a generic method by using a type variable.
        /// </summary>
        /// <param name="instance">Instance of the type containing the generic method</param>
        /// <param name="genericType">Generic type of the method</param>
        /// <param name="methodName">Method name</param>
        /// <param name="parameters">Method parameters (optional)</param>
        /// <returns></returns>
        public static object InvokeGeneric(
            object instance, 
            Type genericType, 
            string methodName, 
            object[] parameters)
        {
            var t = instance.GetType();
            var mi = t.GetMethod(methodName);
            var mig = mi.MakeGenericMethod(genericType);
            return mig.Invoke(instance, parameters);
        }
    }
}
