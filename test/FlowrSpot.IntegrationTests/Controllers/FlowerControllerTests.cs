using Xunit;
using FlowrSpot.Domain.Entities;
using FlowrSpot.Dtos;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;

namespace FlowrSpot.IntegrationTests.Controllers
{
    public class FlowerControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public FlowerControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Fact]
        public async Task GetAll_Always_ReturnsAllFlowers()
        {
            var flowers = new Flower[]
            {
                new(){ Id = new Guid("d3fbb2d7-e71c-4e3d-8168-601b65a4f239"), Name = "Rose", Description = "Red rose", ImageUrl = "/some/url1.jpg" },
                new(){ Id = new Guid("277111d6-13df-49bb-b324-c0408fe0f912"), Name = "Tulip", Description = "Yellow tulip", ImageUrl = "/some/url2.jpg" },
            }.AsEnumerable();

            _factory.FlowerRepositoryMock.Setup(f => f.GetFlowersAsync()).ReturnsAsync(flowers);

            var response = await _client.GetAsync("api/v1/flower");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<FlowerDto>>(await response.Content.ReadAsStringAsync());

            Assert.Collection(data,
                f =>
                {
                    Assert.Equal(new Guid("d3fbb2d7-e71c-4e3d-8168-601b65a4f239"), f.Id);
                    Assert.Equal("Rose", f.Name);
                    Assert.Equal("Red rose", f.Description);
                    Assert.Equal("/some/url1.jpg", f.ImageUrl);
                },
                f =>
                {
                    Assert.Equal(new Guid("277111d6-13df-49bb-b324-c0408fe0f912"), f.Id);
                    Assert.Equal("Tulip", f.Name);
                    Assert.Equal("Yellow tulip", f.Description);
                    Assert.Equal("/some/url2.jpg", f.ImageUrl);
                }
            );
        }

        [Fact]
        public async Task GetFlowerById_ForNonAuthenticatedUser_ReturnsUnauthorized()
        {
            
            var flower = new Flower() { Id = new Guid("277111d6-13df-49bb-b324-c0408fe0f912"), Name = "Tulip", Description = "Yellow tulip", ImageUrl = "/some/url2.jpg" };

            Guid id = new Guid("277111d6-13df-49bb-b324-c0408fe0f912");
            _factory.FlowerRepositoryMock.Setup(f => f.GetFlowerAsync(id).Result).Returns(flower);

            var response = await _client.GetAsync("api/v1/flower/277111d6-13df-49bb-b324-c0408fe0f912");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetFlowerById_IfExists_ReturnsFlower()
        {
            // arrange
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder
                .ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                    services.AddAuthorization(opts =>
                    {
                        opts.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes("Test")
                            .RequireAuthenticatedUser()
                            .Build();
                    });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            Guid id = new Guid("277111d6-13df-49bb-b324-c0408fe0f912");

            var flower = new Flower() { Id = id, 
                Name = "Tulip", 
                Description = "Yellow tulip", 
                ImageUrl = "/some/url2.jpg" };

            _factory.FlowerRepositoryMock.Setup(f => f.GetFlowerAsync(id).Result).Returns(flower);

            // act
            var response = await _client.GetAsync("api/v1/flower/277111d6-13df-49bb-b324-c0408fe0f912");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<FlowerDto?>(await response.Content.ReadAsStringAsync());

            Assert.Equal("277111d6-13df-49bb-b324-c0408fe0f912", data.Id.ToString());
            Assert.Equal("Tulip", data.Name);
            Assert.Equal("Yellow tulip", data.Description);
            Assert.Equal("/some/url2.jpg", data.ImageUrl);
        }

        [Fact]
        public async Task GetFlowerById_IfMissing_ReturnsNotFound()
        {
            // arrange
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder
                .ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                    services.AddAuthorization(opts =>
                    {
                        opts.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes("Test")
                            .RequireAuthenticatedUser()
                            .Build();
                    });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            var flower = new Flower() { Id = new Guid("277111d6-13df-49bb-b324-c0408fe0f912"), Name = "Tulip", Description = "Yellow tulip", ImageUrl = "/some/url2.jpg" };

            Guid id = new Guid("277111d6-13df-49bb-b324-c0408fe0f913");

            // act
            _factory.FlowerRepositoryMock.Setup(f => f.GetFlowerAsync(id).Result).Returns(flower);

            var response = await _client.GetAsync("api/v1/flower/277111d6-13df-49bb-b324-c0408fe0f912");

            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateFlower_WithValidData_SavesFlower()
        {
            // arrange
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder
                .ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                    services.AddAuthorization(opts =>
                    {
                        opts.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes("Test")
                            .RequireAuthenticatedUser()
                            .Build();
                    });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            var newFlower = new CreateFlowerRequest { Name = "Sunflower", Description = "Yellow sunflower", ImageUrl = "/some/url3.jpg" };



            _factory.FlowerRepositoryMock.Setup(r => r.AddFlowerAsync(It.Is<Flower>(
                    f => f.Name == "Sunflower" &&
                         f.Description == "Yellow sunflower" &&
                         f.ImageUrl == "/some/url3.jpg")))
                .Verifiable();

            // act
            var response = await _client.PostAsync("api/v1/flower/", JsonContent.Create(newFlower));


            // assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            _factory.FlowerRepositoryMock.VerifyAll();
        }

    }
}