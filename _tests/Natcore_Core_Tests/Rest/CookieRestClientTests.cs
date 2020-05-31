using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Natcore.Core.Rest;
using Natcore.Core.Rest.Special;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Natcore_Core_Tests.Rest
{
    [TestClass]
    public class CookieRestClientTests
    {
        [TestMethod]
        public async Task Cookies_added_to_request()
        {
            var baseClient = new Mock<IRestClient>();
            baseClient.Setup(x => x.SendAsync(It.IsAny<IRequest>())).ReturnsAsync(new RestResponse());

            var client = new CookiesRestClient(baseClient.Object);
            var request = new RestRequest();

            client.AddCookie("key 1", "value 1")
                .AddCookie("key 2", "value 2");

            await client.SendAsync(request);

            request.Headers.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                ["cookie"] = "key 1=value 1; key 2=value 2"
            });
        }

        [TestMethod]
        public async Task Response_cookies_set_in_client()
        {
            var baseClient = new Mock<IRestClient>();
            baseClient.Setup(x => x.SendAsync(It.IsAny<IRequest>())).ReturnsAsync(new RestResponse()
            {
                Headers = new Dictionary<string, string[]>
                {
                    ["Set-Cookie"] = new[]
                    {
                        "c1=v1; path=/; httponly",
                        "c2=v2"
                    }
                }
            });

            var client = new CookiesRestClient(baseClient.Object);
            var request = new RestRequest();

            await client.SendAsync(request);

            client.Cookies.Should()
                .BeEquivalentTo(new[]
                {
                    new Cookie { Key = "c1", Value = "v1", Path = "/", HttpOnly = true },
                    new Cookie { Key = "c2", Value = "v2" }
                });
        }
    }
}
