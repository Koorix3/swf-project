using System;
using TestScenarioFramework;
using TestScenarioFramework.Export;

namespace LocalDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            // IOC types
            JsonExporter exporter = new JsonExporter();

            var ts = new TestScenario("hello_world2", exporter);
            var someMovie = ts.GetEntity<Entites.Movie>();

            //var rdg = new RandomDataGenerator();

            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(rdg.GetDecimal(50, 60));
            //}

            Console.WriteLine(Utils.DebugUtility.EntityToString(someMovie));
            //ts.Save();
        }
    }
}
