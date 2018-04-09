using System;
using System.Reflection;
using System.Linq;
using TestScenarioFramework.Attributes;
using System.Collections;
using System.Globalization;
using System.IO;
using TestScenarioFramework.Export;

namespace TestScenarioFramework
{
    public class TestScenario
    {
        private const int MaxLevelOfRecursion = 10;

        private string _name;
        private RandomDataGenerator _rdg;
        private IExporter _exporter;

        public TestScenario(string name, IExporter exporter)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name");

            _name = name;
            _rdg = new RandomDataGenerator();

            _exporter = exporter;
            _exporter.Setup(string.Concat(
                Directory.GetCurrentDirectory(),
                Path.DirectorySeparatorChar,
                "test-scenario-data",
                Path.DirectorySeparatorChar,
                _name,
                ".json"));

            if (!_exporter.IsNew) _exporter.Load();
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
            if (!_exporter.IsNew) return;
            _exporter.Save();
        }
        
        private object GetEntity(Type t, int levelOfRecursion)
        {
            // Return persisted entity in read mode ...
            if (!_exporter.IsNew)
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
            if (levelOfRecursion == 0)
                Utils.ReflectionUtility.InvokeGeneric(_exporter, t, "Push", new object[] { entity });

            return entity;
        }

    }
}
