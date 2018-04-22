namespace TestScenarioFramework.Export
{
    /// <summary>
    /// Defined the contract for implementing testscenario exporter classes.
    /// </summary>
    public interface IExporter
    {
        /// <summary>
        /// Defines whether the exporter created new data or loaded an existing scenario.
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// Specifies the file path used by exporters that use the file system.
        /// </summary>
        /// <param name="filePath">File path of the persisted scenario file</param>
        void Setup(string filePath);

        /// <summary>
        /// Pushes a new object on the generated field data stack.
        /// </summary>
        /// <typeparam name="T">Object type used for generation</typeparam>
        /// <param name="o">Object to be pushed on the stack</param>
        void Push<T>(T o);

        /// <summary>
        /// Pops an object from the generated (or loaded) field data stack.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <returns>New instance or value</returns>
        T Pop<T>();

        /// <summary>
        /// Persists a testscenario.
        /// </summary>
        void Save();

        /// <summary>
        /// Loads a testscenario.
        /// </summary>
        void Load();
    }
}
