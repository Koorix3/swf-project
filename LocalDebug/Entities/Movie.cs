using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestScenarioFramework.Attributes;


namespace LocalDebug.Entites
{
    [TestScenarioEntity]
    public class Movie
    {
        [TestScenarioMember]
        public int MovieId { get; set; }

        [TestScenarioMember]
        public string Title { get; set; }

        [TestScenarioMember(Min = "1970-01-01", Max = "2018-04-01")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [TestScenarioMember]
        public string Genre { get; set; }

        [TestScenarioMember(Min = 50, Max = 60)]
        public decimal Price { get; set; }

        [TestScenarioMember(Multiplicity = 5)]
        public virtual List<Actor> Actors { get; set; }
        public virtual Actor Lead { get; set; }
        
    }
}
