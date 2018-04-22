using System;
using System.Reflection;
using System.Linq;
using TestScenarioFramework.Attributes;
using System.IO;
using TestScenarioFramework.Export;

namespace TestScenarioFramework
{
    /// <summary>
    /// Manages test scenarios and mock data.
    /// </summary>
    public class TestScenario
    {
        private const int MaxLevelOfRecursion = 10;

        private string _name;
        private RandomDataGenerator _rdg;
        private IExporter _exporter;

        /// <summary>
        /// Initializes a new TestScenario instance with a specified name and exporter.
        /// </summary>
        /// <param name="name">The name of the scenario and, when used, the persisted file</param>
        /// <param name="exporter">The exporter used when persisting scenarios</param>
        public TestScenario(string name, IExporter exporter)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            _name = name;
            _rdg = new RandomDataGenerator();

            _exporter = exporter;

            if (_exporter != null)
            {
                _exporter.Setup(string.Concat(
                    Directory.GetCurrentDirectory(),
                    Path.DirectorySeparatorChar,
                    "test-scenario-data",
                    Path.DirectorySeparatorChar,
                    _name,
                    ".json"));

                if (!_exporter.IsNew) _exporter.Load();
            }
            
        }
        
        /// <summary>
        /// Creates/loads entity of a certain type.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>A new instance of the specified type</returns>
        public T GetEntity<T>()
        {
            return (T)GetEntity(typeof(T));
        }

        /// <summary>
        /// Creates/loads entity of a certain type.
        /// </summary>
        /// <param name="t">Entity runtime type</param>
        /// <returns>A new instance of the specified type</returns>
        public object GetEntity(Type t)
        {
            return GetEntity(t, 0);
        }

        /// <summary>
        /// Tries to persist the scenario, using the specified IExporter.
        /// </summary>
        public void Save()
        {
            if (_exporter != null && !_exporter.IsNew) return;
            _exporter.Save();
        }

        /// <summary>
        /// Recursive function to create new entites.
        /// </summary>
        /// <param name="t">Entity type</param>
        /// <param name="levelOfRecursion">Current numer of recursions</param>
        /// <returns>A new instance of the specified type</returns>
        private object GetEntity(Type t, int levelOfRecursion)
        {
            // Return persisted entity in read mode ...
            if (_exporter != null && !_exporter.IsNew)
                return Utils.ReflectionUtility.InvokeGeneric(_exporter, t, "Pop", null);

            // ... or create new entity
            if (t.GetCustomAttributes<TestScenarioEntity>().Count() == 0)
                throw new TestScenarioException(
                    $"Type \"{t.ToString()}\" doesn't contain a \"TestScenarioEntity\" attribute.");

            if (levelOfRecursion >= MaxLevelOfRecursion)
                return null;
                //throw new TestScenarioException("Max. number of recusions exceeded.");

            var entity = Activator.CreateInstance(t);

            _rdg.PopulateObjectFields(
                entity, 
                (Type et) => GetEntity(et, levelOfRecursion + 1));

            // Register entity with exporter
            if (_exporter != null && levelOfRecursion == 0)
                Utils.ReflectionUtility.InvokeGeneric(_exporter, t, "Push", new object[] { entity });

            return entity;
        }

    }
}
