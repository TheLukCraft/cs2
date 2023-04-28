using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;
using Newtonsoft.Json;
using WebAPI.Wrappers;
using Application.Dto.Post;
using FluentAssertions;
using System.Net;

namespace EndToEndTests.Controllers
{
    public class PostsControllerTest
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public PostsControllerTest()
        {
            //Arrange
            var projectDir = Helper.GetProjectPath("", typeof(Program).GetTypeInfo().Assembly);
            server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.Development.json")
                    .Build()
                )
                .UseStartup<Program>());
            client = server.CreateClient();
        }

        [Fact]
        public async Task fetching_posts_should_return_not_empty_collection()
        {
            //Act
            var response = await client.GetAsync(@"/Posts");
            var content = await response.Content.ReadAsStringAsync();
            var pagedResponse = JsonConvert.DeserializeObject<PagedResponse<IEnumerable<PostDto>>>(content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            pagedResponse.Data.Should().NotBeNullOrEmpty();
        }
    }
}