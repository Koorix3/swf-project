using System;
using System.Collections.Generic;
using TestScenarioFramework.Attributes;

namespace LocalDebug.Entites
{
    [TestScenarioEntity]
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }

        [TestScenarioMember(Min = "1970-01-01", Max = "2018-04-01")]
        public DateTime ReleaseDate { get; set; }

        [TestScenarioMember(Min = 50, Max = 60)]
        public decimal Price { get; set; }

        [TestScenarioMember(Multiplicity = 5)]
        public virtual List<Actor> Actors { get; set; }

        public virtual Actor Lead { get; set; }
    }
}
