using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Natcore.Core.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Natcore_Core_Tests.Rest
{
    [TestClass]
    public class HeaderableRestClientTests
    {
        [TestMethod]
        public async Task Headers_are_added_to_request()
        {
            var baseClient = Mock.Of<IRestClient>();
            var headerableClient = new HeaderableRestClient(baseClient);
            var request = new RestRequest();

            request.SetHeader("request header 1", "value 1")
                .SetHeader("request header 2", "value 2");

            headerableClient.SetHeader("client header 1", "value 1");
            headerableClient.SetHeader("client header 2", "value 2");

            await headerableClient.SendAsync(request);

            request.Headers.Should().HaveCount(4);

            request.Headers.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                ["request header 1"] = "value 1",
                ["request header 2"] = "value 2",
                ["client header 1"] = "value 1",
                ["client header 2"] = "value 2",
            });
        }
    }
}
