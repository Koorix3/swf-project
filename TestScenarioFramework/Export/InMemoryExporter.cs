using System;
using System.Collections.Generic;
using System.Text;

namespace TestScenarioFramework.Export
{
    public class InMemoryExporter : IExporter
    {
        private static Dictionary<string, List<object>> _cache;
        private List<object> _objects;
        private string _name;
        private int _index;

        public bool IsNew
        {
            get
            {
                return _cache == null || !_cache.ContainsKey(_name);
            }
        }

        public void Load()
        {
            if (_cache != null && _cache.ContainsKey(_name))
                _objects = _cache[_name];
        }

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

        public T Pop<T>()
        {
            return (T)_objects[_index++];
        }

        public void Push<T>(T obj)
        {
            if (_objects == null) _objects = new List<object>();
            _objects.Add(obj);
        }
        
        public void Setup(string filePath)
        {
            _name = filePath;
        }
    }
}
