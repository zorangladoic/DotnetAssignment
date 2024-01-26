using Xunit;
using FlowrSpot.Application.Services;
using FlowrSpot.Domain.Entities;
using FlowrSpot.WebAPI.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using System.Text.Encodings.Web;

namespace FlowrSpot.UnitTests.BasicAuthentication
{
    public class BasicAuthenticationHandlerTest
    {
        private readonly Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _options;
        private readonly Mock<ILoggerFactory> _logger;
        private readonly Mock<UrlEncoder> _encoder;
        private readonly Mock<ISystemClock> _clock;
        private readonly Mock<IUserService> _userService;

        public BasicAuthenticationHandlerTest()
        {
            _options = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();

            _options
                .Setup(x => x.Get(It.IsAny<string>()))
                .Returns(new AuthenticationSchemeOptions());

            var logger = new Mock<ILogger<BasicAuthenticationHandler>>();
            _logger = new Mock<ILoggerFactory>();
            _logger
                .Setup(x => x.CreateLogger(It.IsAny<string>()))
                .Returns(logger.Object);

            _encoder = new Mock<UrlEncoder>();
            _clock = new Mock<ISystemClock>();
            _userService = new Mock<IUserService>();
        }

        [Fact]
        public async Task AuthorizationHeaderNotProvided_ShouldReturnInvalidAuthorizationKey()
        {
            var context = new DefaultHttpContext();

            var _handler = new BasicAuthenticationHandler(_userService.Object, _options.Object, _logger.Object, _encoder.Object, _clock.Object);

            await _handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationDefaults.AuthenticationSchemes, null, typeof(BasicAuthenticationHandler)), context);
            var result = await _handler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.Equal("Missing Authorization key", result.Failure.Message);
        }

        [Fact]
        public async Task InvalidCredentials_ShouldReturnFail()
        {
            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("Basic d29ybmdVc2VyMTIzOndyb25nUGFzc3dvcmQxMjM=");
            context.Request.Headers.Append(HeaderNames.Authorization, authorizationHeader);

            _userService
                .Setup(user => user.Authenticate("testUser", "testPassword"))
                .ReturnsAsync(new User { Id = new Guid("e2983a53-91d5-42c1-bcb1-4e3ff419b81f"), Username = "testUser" });

            var _handler = new BasicAuthenticationHandler(_userService.Object, _options.Object, _logger.Object, _encoder.Object, _clock.Object);

            await _handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationDefaults.AuthenticationSchemes, null, typeof(BasicAuthenticationHandler)), context);
            var result = await _handler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.Equal("The username or password is not correct", result.Failure.Message);
        }

        [Fact]
        public async Task ValidCredentials_ShouldReturnSuccess()
        {
            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("Basic dGVzdFVzZXI6dGVzdFBhc3N3b3Jk");
            context.Request.Headers.Append(HeaderNames.Authorization, authorizationHeader);

            _userService
                .Setup(user => user.Authenticate("testUser", "testPassword"))
                .ReturnsAsync(new User { Id = new Guid("e2983a53-91d5-42c1-bcb1-4e3ff419b81f"), Username = "testUser" });

            var _handler = new BasicAuthenticationHandler(_userService.Object, _options.Object, _logger.Object, _encoder.Object, _clock.Object);

            await _handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationDefaults.AuthenticationSchemes, null, typeof(BasicAuthenticationHandler)), context);
            var result = await _handler.AuthenticateAsync();

            Assert.True(result.Succeeded);
            Assert.Equal(3, result.Principal.Claims.Count());
            Assert.Equal("testUser", result.Principal.Claims.First().Value);
            Assert.Equal("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", result.Principal.Claims.First().Type);
            Assert.Equal("e2983a53-91d5-42c1-bcb1-4e3ff419b81f", result.Principal.Claims.Skip(1).First().Value);
            Assert.Equal("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", result.Principal.Claims.Skip(1).First().Type);
            Assert.Equal("testUser", result.Principal.Claims.Last().Value);
        }

    }
}