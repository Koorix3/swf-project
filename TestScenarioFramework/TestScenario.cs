using System;
using System.Reflection;
using System.Linq;
using TestScenarioFramework.Attributes;
using System.Text;
using System.Collections;

namespace TestScenarioFramework
{
    public class TestScenario
    {
        private const int MaxLevelOfRecursion = 10;

        private string _name;
        private RandomDataGenerator _rdg;

        public TestScenario(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            _name = name;
            _rdg = new RandomDataGenerator();
        }

        public T GetEntity<T>()
        {
            return (T)GetEntity(typeof(T));
        }

        public object GetEntity(Type t)
        {
            return GetEntity(t, 0);
        }
        
        private object GetEntity(Type t, int levelOfRecursion)
        {
            if (t.GetCustomAttributes<TestScenarioEntity>().Count() == 0)
                throw new TestScenarioException(
                    $"Type \"{t.ToString()}\" doesn't contain an \"TestScenarioEntity\" attribute.");

            if (levelOfRecursion >= MaxLevelOfRecursion)
                throw new TestScenarioException("Max. number of recusions exceeded.");

            var entity = Activator.CreateInstance(t);

            PopulateEntityProperties(entity, levelOfRecursion);

            return entity;
        }

        private void PopulateEntityProperties(object instance, int levelOfRecursion)
        {
            Type t = instance.GetType();

            foreach (var pi in t.GetProperties())
            {
                //Console.WriteLine(pi.PropertyType);
                switch (pi.PropertyType.ToString())
                {
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        pi.SetValue(instance, _rdg.GetInteger(100));
                        break;

                    case "System.Decimal":
                        pi.SetValue(instance, _rdg.GetInteger(10000) / 100m);
                        break;

                    case "System.Single":
                    case "System.Double":
                        pi.SetValue(instance, _rdg.GetInteger(10000) / 100f);
                        break;
                        
                    case "System.DateTime":
                        pi.SetValue(instance, _rdg.GetDateTime(DateTime.Now.AddMonths(-1), DateTime.Now));
                        break;

                    case "System.String":
                        pi.SetValue(instance, _rdg.GetString(10));
                        break;

                    default:

                        if (pi.PropertyType.IsValueType)
                            break;

                        if (typeof(IList).IsAssignableFrom(pi.PropertyType))
                        {
                            // Create new enumerable
                            var list = (IList)Activator.CreateInstance(pi.PropertyType);
                            Type innerType = pi.PropertyType.GetGenericArguments()[0];
                            
                            // Create some liste elements
                            for (int i = 0; i < 10; i++)
                            {
                                list.Add(GetEntity(innerType, levelOfRecursion + 1));
                            }

                            pi.SetValue(instance, list);
                        }
                        //else
                        //    Console.WriteLine("nah" + pi.Name);

                        break;
                }

            }
        }

        /*
        private Assembly _assembly;
        private string _name;

        public TestScenario(Assembly assembly, string name)
        {
            if (assembly == null)
                throw new ArgumentException("assembly");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            _name = name;
            _assembly = assembly;

            Initialize();
        }

        private void Initialize()
        {
            var scenarioTypes = 
                from t in _assembly.GetTypes()
                where t.GetCustomAttributes(typeof(TestScenarioEntity)).Count() > 0
                select t;



        }*/
    }
}
