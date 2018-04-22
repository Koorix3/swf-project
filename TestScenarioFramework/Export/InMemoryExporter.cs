using System.Collections.Generic;

namespace TestScenarioFramework.Export
{
    /// <summary>
    /// Persists test scenarios in memory (for testing purposes).
    /// </summary>
    public class InMemoryExporter : IExporter
    {
        private static Dictionary<string, List<object>> _cache;
        private List<object> _objects;
        private string _name;
        private int _index;

        /// <summary>
        /// Specifies whether the exporter is newly initiated.
        /// </summary>
        public bool IsNew
        {
            get
            {
                return _cache == null || !_cache.ContainsKey(_name);
            }
        }

        /// <summary>
        /// Loads a test scenario from memory.
        /// </summary>
        public void Load()
        {
            if (_cache != null && _cache.ContainsKey(_name))
                _objects = _cache[_name];
        }

        /// <summary>
        /// Saves a test scenario in memory.
        /// </summary>
        public void Save()
        {
            if (_cache == null) _cache = 
                    new Dictionary<string, List<object>>();

            if (_cache.ContainsKey(_name))
            {
                _cache.Add(_name, _objects);
            }
            else
            {
                _cache[_name] = _objects;
            }
        }

        /// <summary>
        /// Pops a new object from the stack.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <returns>New object instance or value</returns>
        public T Pop<T>()
        {
            return (T)_objects[_index++];
        }

        /// <summary>
        /// Pushes a new value on the stack.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object instance or value</param>
        public void Push<T>(T obj)
        {
            if (_objects == null) _objects = new List<object>();
            _objects.Add(obj);
        }
        
        /// <summary>
        /// Specifies the key for retrieving in-memory data.
        /// </summary>
        /// <param name="filePath">In-memory key for the generated data stack</param>
        public void Setup(string filePath)
        {
            _name = filePath;
        }
    }
}
