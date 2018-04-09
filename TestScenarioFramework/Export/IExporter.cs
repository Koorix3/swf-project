namespace TestScenarioFramework.Export
{
    public interface IExporter
    {
        bool IsNew { get; }
        void Setup(string filePath);
        void Push<T>(T o);
        T Pop<T>();
        void Save();
        void Load();
    }
}
