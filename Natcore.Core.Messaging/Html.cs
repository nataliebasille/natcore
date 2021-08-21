using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public static class Html
    {
        public static HtmlPart Div(HtmlPart content) => Div(new(), content);
        public static HtmlPart Div(HtmlStyles styles, HtmlPart content) => new HtmlTag("div", content)
        {
            Styles = styles
        };

        public static HtmlPart Span(HtmlPart content) => Span(new(), content);
        public static HtmlPart Span(HtmlStyles styles, HtmlPart content) 
            => new HtmlTag("span", content) { Styles = styles };

        public static HtmlPart A((string label, string url) props) => A(new(), props);
        public static HtmlPart A(HtmlStyles styles, (string label, string url) props)
            => new HtmlLink(props.label, props.url) { Styles = styles };

        public static HtmlPart B(string text) => B(new(), text);
        public static HtmlPart B(HtmlStyles styles, string text) 
            => B(styles, Text(text));
        public static HtmlPart B(HtmlPart content) => B(new(), content);
        public static HtmlPart B(HtmlStyles styles, HtmlPart content) 
            => new HtmlTag("b", content) { Styles = styles };

        public static HtmlPart U(string text) => U(new(), text);
        public static HtmlPart U(HtmlStyles styles, string text)
            => U(styles, Text(text));
        public static HtmlPart U(HtmlPart content) => U(new(), content);
        public static HtmlPart U(HtmlStyles styles, HtmlPart content)
            => new HtmlTag("u", content) { Styles = styles };

        public static HtmlPart Br => new HtmlClosedTag("br");
        public static HtmlPart Hr => new HtmlClosedTag("hr");

        public static HtmlPart Ul(params HtmlListItem[] items) => Ul(new(), items);
        public static HtmlPart Ul(HtmlStyles styles, params HtmlListItem[] items)
            => new HtmlUnorderedList(items) { Styles = styles };

        public static HtmlPart Ol(params HtmlListItem[] items) => Ol(new(), items);
        public static HtmlPart Ol(HtmlStyles styles, params HtmlListItem[] items) 
            => new HtmlOrderedList(items) { Styles = styles };

        public static HtmlListItem Li(string text) => Li(new(), text);
        public static HtmlListItem Li(HtmlPart content) => Li(new(), content);
        public static HtmlListItem Li(HtmlStyles styles, string text) => Li(styles, Text(text));
        public static HtmlListItem Li(HtmlStyles styles, HtmlPart content)
            => new(content) { Styles = styles };


        public static HtmlPart Nothing => new HtmlNothing();

        public static HtmlPart Text(string text) => new HtmlText(text);

        public static HtmlPart Collection(params HtmlPart[] items)
            => new HtmlPartCollection(items);
    }
}
