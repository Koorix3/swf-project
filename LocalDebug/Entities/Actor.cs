using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestScenarioFramework.Attributes;

namespace LocalDebug.Entites
{
    [TestScenarioEntity]
    public class Actor
    {
        public int ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int MovieId { get; set; }

        // Avoid cyclic references for mock data generation!
        [TestScenarioMember(Exclude = true)]
        public Movie Movie { get; set; }
    }
}
