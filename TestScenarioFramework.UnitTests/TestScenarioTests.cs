using System;
using Xunit;
using TestScenarioFramework;
using TestScenarioFramework.Export;

namespace TestScenarioFramework.UnitTests
{
    public class TestScenarioTests
    {
        [Fact]
        public void CreatesEntitiesWithFieldValues()
        {
            var ts = new TestScenario("tmp", null);
            var someMovie = ts.GetEntity<Entites.Movie>();

            Assert.True(someMovie.Title != null);
        }

        [Fact]
        public void CheckDateRangeAttribute()
        {
            var ts = new TestScenario("tmp", null);
            var someMovie = ts.GetEntity<Entites.Movie>();

            Assert.True(someMovie.ReleaseDate.Year == 2000);
        }

        [Fact]
        public void CheckListCreation()
        {
            var ts = new TestScenario("tmp", null);
            var someMovie = ts.GetEntity<Entites.Movie>();

            Assert.True(someMovie.Actors.Count > 0);
        }

        [Fact]
        public void CheckReferencedEntityCreation()
        {
            var ts = new TestScenario("tmp", null);
            var someMovie = ts.GetEntity<Entites.Movie>();

            Assert.True(someMovie.Lead != null);
        }

        [Fact]
        public void CheckExporterSave()
        {
            InMemoryExporter exporter = new InMemoryExporter();

            var ts = new TestScenario("test", exporter);
            var someMovie = ts.GetEntity<Entites.Movie>();

            someMovie.Title = "Back to the Future";
            exporter.Save();

            Assert.True(!exporter.IsNew);
        }

        [Fact]
        public void CheckExporterLoad()
        {
            InMemoryExporter exporter = new InMemoryExporter();

            var ts = new TestScenario("test", exporter);
            var someMovie = ts.GetEntity<Entites.Movie>();

            Assert.Equal("Back to the Future", someMovie.Title);
        }

    }
}
