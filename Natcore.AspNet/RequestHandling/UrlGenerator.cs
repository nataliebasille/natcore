using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Natcore.AspNet
{
    public interface IUrlGenerator
    {
		string BaseUrl { get; }
        string GenerateUrl(string action, string controller, object routeValues);
        string GenerateUrl(string relativeUrl);
    }

    public class UrlGenerator : IUrlGenerator
    {
        private readonly IUrlHelper _urlhelper;
        public UrlGenerator(IUrlHelper urlHelper)
        {
            _urlhelper = urlHelper;

			HttpRequest request = urlHelper.ActionContext.HttpContext.Request;
			BaseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
		}

		public string BaseUrl { get; }

        public string GenerateUrl(string action, string controller, object routeValues)
            => _urlhelper.Action(new UrlActionContext
            {
                Action = action?.ToLower(),
                Controller = controller?.ToLower(),
                Values = routeValues
            });

        public string GenerateUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return "";

            if (relativeUrl[0] != '~' && relativeUrl.Length > 1)
            {
                if (relativeUrl[1] == '/')
                    relativeUrl = $"~{relativeUrl}";
                else
                    relativeUrl = $"~/{relativeUrl}";
            }
            else if (relativeUrl[0] == '/')
                relativeUrl = "~";
            else
                relativeUrl = $"~/{relativeUrl}";

            return _urlhelper.Content(relativeUrl);
        }
    }
}
