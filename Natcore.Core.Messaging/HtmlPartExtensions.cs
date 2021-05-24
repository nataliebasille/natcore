using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Natcore.Core.Messaging
{
    public static class HtmlPartExtensions
    {
        public static BodyBuilder CreateBody(this HtmlPart part)
        {
            List<LinkedResource> resources = new List<LinkedResource>();

            BodyBuilder body = new()
            {
                HtmlBody = CreateBodyString(part, resources)
            };

            for(int i = 0; i < resources.Count; i++)
            {
                body.LinkedResources.Add(resources[i].Url).ContentId = resources[i].ID;
            }

            return body;
        }

        private static string CreateBodyString(HtmlPart part, List<LinkedResource> resources)
            => part switch
            {
                HtmlTable { Rows: var rows, Styles: var styles, CellSpacing: string cellspacing }
                    => $"<table style=\"{styles.CreateStyles()}\" cellspacing=\"{cellspacing}\">{string.Join("", rows.Select(r => CreateBodyString(part, resources)))}</table>",
                HtmlTableRow { Items: var items, Styles: var styles }
                    => $"<tr style=\"{styles.CreateStyles()}\">{string.Join("", items.Select(i => CreateBodyString(i, resources)))}</tr>",
                HtmlTableRowItem { Content: var content, Styles: var styles }
                    => $"<td style=\"{styles.CreateStyles()}\">{CreateBodyString(content, resources)}</td>",
                HtmlLink { ActionUrl: var url, Label: var label, Styles: var styles }
                    => $"<a href=\"{url}\" style=\"text-decoration:none;{styles.CreateStyles()}\">{label}</a>",
                HtmlImage image => CreateImageString(image, resources),
                HtmlTag { Tag: string tag, Styles: var styles, Content: var content }
                    => $"<{tag} style=\"{styles.CreateStyles()}\">{CreateBodyString(content, resources)}</{tag}>",
                HtmlPartCollection { Parts: var parts }
                    => string.Join("", parts.Select(p => CreateBodyString(p, resources))),
                HtmlText { Text: string text }
                    => text,
                HtmlClosedTag { Tag: string tag, Styles: var styles }
                    => $"<{tag} style=\"{styles.CreateStyles()}\" />",
                HtmlUnorderedList { Items: var items, Styles: var styles }
                    => $"<ul style=\"{styles.CreateStyles()}\">{string.Join("", items.Select(i => CreateBodyString(i, resources)))}\"></ul>",
                HtmlOrderedList { Items: var items, Styles: var styles }
                    => $"<ol style=\"{styles.CreateStyles()}\">{string.Join("", items.Select(i => CreateBodyString(i, resources)))}</ol>",
                HtmlListItem { Content: HtmlPart content, Styles: var styles }
                    => $"<li style=\"{styles.CreateStyles()}\">{CreateBodyString(content, resources)}</li>",
                _ => ""
            };

        private static string CreateImageString(HtmlImage image, List<LinkedResource> resources)
        {
            resources.Add(image.ID, image.ImagePath);
            return $"<img src=\"cid:{image.ID}\" width=\"{image.Width}\" height=\"{image.Height}\" />";
        }
    }
}
