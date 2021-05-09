using System;

namespace Natcore.Core.Messaging
{
    public abstract record HtmlPart;

    public abstract record HtmlPartWithStyles : HtmlPart
    {
        public HtmlStyles Styles { get; init; } = new();
    }

    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public enum VerticalAlignment
    {
        Top,
        Middle,
        Bottom
    }

    public record HtmlContainer(params HtmlRow[] Rows) : HtmlPartWithStyles;
    public record HtmlRow(params HtmlRowItem[] Items) : HtmlPartWithStyles;
    
    public record HtmlRowItem(HtmlPart Content) : HtmlPartWithStyles
    {
        public HorizontalAlignment HorizontalAlignment { get; init; } = HorizontalAlignment.Center;

        public VerticalAlignment VerticalAlignment { get; init; } = VerticalAlignment.Middle;
    }

    public record HtmlImage(string ID, string ImagePath, int Width, int Height) : HtmlPart;
    public record HtmlLink(string Label, string ActionUrl): HtmlPartWithStyles;
    public record HtmlPartCollection(HtmlPart[] Parts) : HtmlPart;
    public record HtmlTag(string Tag, HtmlPart Content) : HtmlPartWithStyles;
    public record HtmlText(string Text) : HtmlPart;
    public record HtmlNothing(): HtmlPart;

    public static class Html
    {
        public static HtmlPart Button(string backgroundColor, string textColor, string label, string url)
            => new HtmlLink(label, url)
            {
                Styles = new HtmlStyles
                {
                    BackgroundColor = backgroundColor,
                    Color = textColor,
                    Padding = "8px 22px",
                    TextDecoration = "none",
                    FontSize = "1.5rem",
                    BorderRadius = "5px"
                }
            };

        public static HtmlPart Divider()
            => new HtmlTag(Tag: "div", Content: new HtmlNothing())
            {
                Styles = new HtmlStyles
                { 
                    BackgroundColor = "#bfc2c9", 
                    Height = "1px", 
                    Margin = "0 1rem" 
                }
            };

        public static HtmlPart Heading1(string text, string textColor)
            => new HtmlTag(Tag: "h1", Content: new HtmlText(text))
            {
                Styles = new HtmlStyles
                {
                    Color = textColor
                }
            };

        public static HtmlPart Image(string id, string path, int size)
            => Image(id, path, size, size);

        public static HtmlPart Image(string id, string path, int width, int height)
            => new HtmlImage(id, path, width, height);

        public static HtmlPart Text(string text, HtmlStyles styles = null)
            => new HtmlTag(Tag: "span", Content: new HtmlText(text))
            {
                Styles = styles ?? new HtmlStyles()
            };

        public static HtmlPart MutedText(string text, HtmlStyles styles = null)
            => new HtmlTag(Tag: "span", Content: new HtmlText(text))
            {
                Styles = (styles ?? new HtmlStyles()) with { Color = "#6c757d" }
            };

        public static HtmlPart Copyright()
            => new HtmlText(Text: $"Copyright © {DateTime.Now.Year}, BREW SOFTWARE, INC.");
    }
}
