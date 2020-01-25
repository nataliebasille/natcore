using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Natcore.Core.Rest
{
    public class HeaderableRestClient : IRestClient
    {
        private readonly IRestClient _base;
        public HeaderableRestClient(IRestClient @base)
        {
            _base = @base;
        }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Task<IResponse> SendAsync(IRequest request)
        {
            if(Headers != null)
            {
                foreach (KeyValuePair<string, string> header in Headers)
                    request.SetHeader(header.Key, header.Value);
            }

            return _base.SendAsync(request);
        }
    }
}
