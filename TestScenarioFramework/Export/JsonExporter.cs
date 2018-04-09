using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestScenarioFramework.Export
{
    public class JsonExporter : IExporter
    {
        private string _filePath;
        private JArray _objects;
        private int _index;
        private bool? _isNew;

        public bool IsNew
        {
            get
            {
                if (_isNew == null)
                    _isNew = !File.Exists(_filePath);

                return _isNew.Value;
            }
        }

        public void Setup(string filePath)
        {
            _filePath = filePath;
        }

        // Deserialization in Newtonsoft.Json needs to know all types beforehand.
        // Workaraund: Read entities as JObject-Array and convert late (when the type is known).
        public void Load()
        {
            string jsonText = File.ReadAllText(_filePath);
            _objects = (JArray)JsonConvert.DeserializeObject(jsonText);
        }

        public void Save()
        {
            string dirPath = Path.GetDirectoryName(_filePath);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            string jsonText = JsonConvert.SerializeObject(_objects, Formatting.Indented);

            using (StreamWriter sw = File.CreateText(_filePath))
            {
                sw.Write(jsonText);
            }
        }

        public T Pop<T>()
        {
            return (T)_objects[_index++].ToObject(typeof(T));
        }

        public void Push<T>(T obj)
        {
            if (_objects == null) _objects = new JArray();
            JToken token = JToken.FromObject(obj);
            _objects.Add(token);
        }

       
    }
}
