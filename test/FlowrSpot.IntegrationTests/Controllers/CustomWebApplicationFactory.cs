using FlowrSpot.Application.Repositories;
using FlowrSpot.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FlowrSpot.IntegrationTests.Controllers
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IFlowerRepository> FlowerRepositoryMock { get; }

        public CustomWebApplicationFactory()
        {
            FlowerRepositoryMock = new Mock<IFlowerRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(FlowerRepositoryMock.Object);
            });
        }
    }
}
