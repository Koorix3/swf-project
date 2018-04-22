using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TestScenarioFramework.Export
{
    /// <summary>
    /// Persists test scenario data as JSON in the file system.
    /// </summary>
    public class JsonExporter : IExporter
    {
        private string _filePath;
        private JArray _objects;
        private int _index;
        private bool? _isNew;

        /// <summary>
        /// Specifies, whether the JSON file already exists in the file system.
        /// </summary>
        public bool IsNew
        {
            get
            {
                if (_isNew == null)
                    _isNew = !File.Exists(_filePath);

                return _isNew.Value;
            }
        }

        /// <summary>
        /// Sets up the file path for the JSON file.
        /// </summary>
        /// <param name="filePath">File path for the generated JSON file</param>
        public void Setup(string filePath)
        {
            _filePath = filePath;
        }

        // Deserialization in Newtonsoft.Json needs to know all types beforehand.
        // Workaraund: Read entities as JObject-Array and convert late (when the type is known).
        /// <summary>
        /// Loads test scenario data from specified JSON-file.
        /// </summary>
        public void Load()
        {
            string jsonText = File.ReadAllText(_filePath);
            _objects = (JArray)JsonConvert.DeserializeObject(jsonText);
        }

        /// <summary>
        /// Saves test scenario data to a JSON-file in the file system.
        /// </summary>
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

        /// <summary>
        /// Pops a generated/loaded field value from the data stack.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <returns>New object instance or value</returns>
        public T Pop<T>()
        {
            return (T)_objects[_index++].ToObject(typeof(T));
        }

        /// <summary>
        /// Pushes a new value on the data stack.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="obj">Object instance or value</param>
        public void Push<T>(T obj)
        {
            if (_objects == null) _objects = new JArray();
            JToken token = JToken.FromObject(obj);
            _objects.Add(token);
        }

       
    }
}
