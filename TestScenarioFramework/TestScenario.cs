using System;
using System.Reflection;
using System.Linq;
using TestScenarioFramework.Attributes;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TestScenarioFramework
{
    public class TestScenario
    {
        private const int MaxLevelOfRecursion = 10;
        private const int DefaultListMultiplicity = 10;

        private string _name;
        private RandomDataGenerator _rdg;
        private List<object> _createdEntities;
        private string _filePath;
        private Boolean _createMode;
        private int _entityIndex;
        private JArray _loadedEntites;

        public TestScenario(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            _name = name;
            _rdg = new RandomDataGenerator();
            _filePath = string.Concat(
                Directory.GetCurrentDirectory(),
                Path.DirectorySeparatorChar,
                "test-scenario-data", 
                Path.DirectorySeparatorChar, 
                _name, 
                ".json");

            _createMode = !File.Exists(_filePath);
            
            // Deserialization in Newtonsoft.Json needs to know all types beforehand.
            // Workaraund: Read entities as JObject-Array and convert late (when the type is known).
            if (!_createMode) _loadedEntites = LoadEntities(_filePath);
        }

        private JArray LoadEntities(string filePath)
        {
            string jsonText = File.ReadAllText(filePath);
            return (JArray)JsonConvert.DeserializeObject(jsonText);
        }

        public T GetEntity<T>()
        {
            return (T)GetEntity(typeof(T));
        }

        public object GetEntity(Type t)
        {
            return GetEntity(t, 0);
        }

        public void Save()
        {
            if (!_createMode) return;

            string dirPath = Path.GetDirectoryName(_filePath);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            string jsonText = JsonConvert.SerializeObject(_createdEntities, Formatting.Indented);

            using (StreamWriter sw = File.CreateText(_filePath))
            {
                sw.Write(jsonText);
            }
        }
        
        private object GetEntity(Type t, int levelOfRecursion)
        {
            if (!_createMode) return _loadedEntites[_entityIndex++].ToObject(t);

            if (t.GetCustomAttributes<TestScenarioEntity>().Count() == 0)
                throw new TestScenarioException(
                    $"Type \"{t.ToString()}\" doesn't contain an \"TestScenarioEntity\" attribute.");

            if (levelOfRecursion >= MaxLevelOfRecursion)
                throw new TestScenarioException("Max. number of recusions exceeded.");

            var entity = Activator.CreateInstance(t);

            PopulateEntityProperties(entity, levelOfRecursion);

            if (levelOfRecursion == 0)
            {
                if (_createdEntities == null) _createdEntities = new List<object>();
                _createdEntities.Add(entity);
            }

            return entity;
        }

        private void PopulateEntityProperties(object instance, int levelOfRecursion)
        {
            Type t = instance.GetType();

            foreach (var pi in t.GetProperties())
            {
                var att = pi.GetCustomAttribute<TestScenarioMemberAttribute>();

                //Console.WriteLine(pi.PropertyType);
                switch (pi.PropertyType.ToString())
                {
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        pi.SetValue(instance, _rdg.GetInteger(100));
                        break;

                    case "System.Decimal":
                        { 
                            decimal min = att != null ? Convert.ToDecimal(att.Min) : 0m;
                            decimal max = att != null ? Convert.ToDecimal(att.Max) : 0m;
                        
                            pi.SetValue(
                                instance, 
                                max != 0m ? _rdg.GetDecimal(min, max) : _rdg.GetDecimal(max));

                            break;
                        }
                    case "System.Single":
                    case "System.Double":
                        { 
                            double min = att != null ? Convert.ToDouble(att.Min) : 0d;
                            double max = att != null ? Convert.ToDouble(att.Max) : 0d;

                            pi.SetValue(
                                instance,
                                max != 0d ? _rdg.GetDouble(min, max) : _rdg.GetDouble(max));
                        
                            break;
                        }
                    case "System.DateTime":
                        {
                            DateTime min = DateTime.MinValue;
                            DateTime max = DateTime.MinValue;

                            if (att != null)
                            {
                                DateTime.TryParseExact(
                                    Convert.ToString(att.Min), 
                                    "yyyy-MM-dd", 
                                    CultureInfo.InvariantCulture, 
                                    DateTimeStyles.None, 
                                    out min);

                                DateTime.TryParseExact(
                                    Convert.ToString(att.Max),
                                    "yyyy-MM-dd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out max);
                            }

                            if (min == DateTime.MinValue) min = DateTime.Now.AddMonths(-1);
                            if (max == DateTime.MinValue) max = DateTime.Now;

                            pi.SetValue(instance, _rdg.GetDateTime(min, max));
                            break;

                        }

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
                            int numElements = att != null ? att.Multiplicity : DefaultListMultiplicity;

                            // Create some list elements
                            for (int i = 0; i < numElements; i++)
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
    }
}
