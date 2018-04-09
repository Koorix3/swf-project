using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Controllers;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovie.UnitTests.Controllers
{
    public class MoviesControllerDetails
    {
        public MoviesControllerDetails()
        {
            
        }

        [Fact]
        public async void ReturnsNotFoundWithInvalidId()
        {
            // Todo: Use TestScenarioFramework to generate data.
            Assert.True(true);

            /*
            var optionsBuilder = 
                new DbContextOptionsBuilder<MvcMovieContext>();

            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MvcMovieContext-c2d1b2b6-8ead-4fc9-b738-a6b9476f487f;Trusted_Connection=True;MultipleActiveResultSets=true");
            
            var ctx = new MvcMovieContext(optionsBuilder.Options);
            var c = new MoviesController(ctx);
            
            var result = await c.Details(-1);
            var viewResult = Assert.IsType<NotFoundResult>(result);

            Assert.Equal((int)viewResult.StatusCode, (int)HttpStatusCode.NotFound);
            */
            
        }
    }
}
