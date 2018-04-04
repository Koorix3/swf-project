using System;
using TestScenarioFramework;

namespace LocalDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = new TestScenario("hello_world");
            var someMovie = ts.GetEntity<Entites.Movie>();

            //var rdg = new RandomDataGenerator();

            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(rdg.GetDecimal(50, 60));
            //}

            Console.WriteLine(Utils.DebugUtils.EntityToString(someMovie));
            ts.Save();
        }
    }
}
