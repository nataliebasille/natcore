using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Natcore.Core.Rest.Special
{
    public class CookiesRestClient : IRestClient
    {
        private readonly IRestClient _baseClient;
        private readonly List<ICookie> cookies = new List<ICookie>();

        public CookiesRestClient(IRestClient baseClient)
        {
            _baseClient = baseClient;
        }

        public IEnumerable<ICookie> Cookies => cookies;

        public Task<IResponse> SendAsync(IRequest request)
        {
            request.SetHeader("cookie", CreateCookieString(request));

            return HandleRequest();

            async Task<IResponse> HandleRequest()
            {
                var response = await _baseClient.SendAsync(request);

                if (response.Headers?.ContainsKey("Set-Cookie") ?? false)
                {
                    var cookies_to_set = response.Headers["Set-Cookie"];

                    for(int i = 0; i < cookies_to_set.Length; i++)
                    {
                        var cookie = cookies_to_set[i];

                        cookies.Add(CreateCookie(cookie));
                    }
                }

                return response;
            };
        }

        public void AddCookie(ICookie cookie)
            => cookies.Add(cookie);

        private ICookie CreateCookie(string cookieValue)
        {
            string[] parts = cookieValue.Split(";");

            var cookie = new Cookie();

            for(int i = 0; i < parts.Length; i++)
            {
                var (key, value) = GetKeyValue(parts[i]);

                if (i == 0)
                {
                    cookie.Key = key;
                    cookie.Value = value;
                }
                else if (key == "path")
                    cookie.Path = value;
                else if (key == "httponly")
                    cookie.HttpOnly = true;
                else if (key == "secure")
                    cookie.Secure = true;
                else if (key == "samesite")
                    cookie.SameSite = value;
            }

            return cookie;
        }

        private string CreateCookieString(IRequest request)
        {
            return string.Join("; ", cookies.Select(x => $"{x.Key}={x.Value}"));
        }

        private (string key, string value) GetKeyValue(string str)
        {
            var parts = str.Trim().Split("=");

            return (parts[0], parts.Length > 1 ? parts[1] : "");
        }
    }
}
