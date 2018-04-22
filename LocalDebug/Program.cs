using System;
using TestScenarioFramework;
using TestScenarioFramework.Export;

namespace LocalDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize exporter (For testing. Exporters should be constructed by DI-frameworks.)
            JsonExporter exporter = new JsonExporter();

            // Create a new TestScenario instance
            var scenario = new TestScenario("hello_world2", exporter);

            // Create a random entity on-the-fly
            var someMovie = scenario.GetEntity<Entites.Movie>();
            
            Console.WriteLine(Utils.DebugUtility.EntityToString(someMovie));
            //scenario.Save();
        }
    }
}
