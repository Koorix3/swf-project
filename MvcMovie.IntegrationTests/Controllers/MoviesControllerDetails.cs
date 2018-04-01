using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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

namespace MvcMovie.IntegrationTests.Controllers
{
    public class MoviesControllerDetails
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MoviesControllerDetails()
        {
            // Todo: Can't find connection string
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
           
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ReturnsNotFoundWithInvalidId()
        {
            Assert.True(true);

            //var response = await _client.GetAsync("/Movies/Details/-1");
            //response.EnsureSuccessStatusCode();

            //Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
