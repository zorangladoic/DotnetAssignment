using Moq;
using System.Text.Encodings.Web;

namespace FlowrSpot.UnitTest
{
    public class BasicAuthenticationHandlerTests
    {
        private readonly Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _options;
        private readonly Mock<ILoggerFactory> _logger;
        private readonly Mock<UrlEncoder> _encoder;
        private readonly Mock<ISystemClock> _clock;
        private readonly Mock<IUserService> _userService;

        [Fact]
        public void Test1()
        {

        }
    }
}