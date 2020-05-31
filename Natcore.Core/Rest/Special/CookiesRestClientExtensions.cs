namespace Natcore.Core.Rest.Special
{
    public static class CookiesRestClientExtensions
    {
        public static CookiesRestClient AddCookie(this CookiesRestClient client, string key, string value)
        {
            client.AddCookie(new Cookie
            {
                Key = key,
                Value = value
            });

            return client;
        }
    }
}
