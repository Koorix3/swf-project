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

            /*
            var rnd = new RandomDataGenerator();

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine(rnd.GetDateTime(new DateTime(2018, 3, 1), DateTime.Today));
            }
            */

            Console.WriteLine(someMovie);
        }
    }
}
