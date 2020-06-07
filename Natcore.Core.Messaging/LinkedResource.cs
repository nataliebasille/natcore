using System.Collections.Generic;

namespace Natcore.Core.Messaging
{
    public class LinkedResource
    {
        public string ID { get; set; }

        public string Url { get; set; }
    }

    public static class LinkedResourcesExtensions
    {
        public static List<LinkedResource> Add(this List<LinkedResource> source, string id, string url)
        {
            source.Add(new LinkedResource { ID = id, Url = url });
            return source;
        }
    }
}
