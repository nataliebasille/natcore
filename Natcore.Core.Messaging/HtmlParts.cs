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

    public record HtmlTable(params HtmlTableRow[] Rows) : HtmlPartWithStyles
    {
        public string CellSpacing { get; init; }
    }

    public record HtmlTableRow(params HtmlTableRowItem[] Items) : HtmlPartWithStyles;
    
    public record HtmlTableRowItem(HtmlPart Content) : HtmlPartWithStyles
    {
        public HorizontalAlignment HorizontalAlignment { get; init; } = HorizontalAlignment.Left;

        public VerticalAlignment VerticalAlignment { get; init; } = VerticalAlignment.Middle;
    }

    public record HtmlImage(string ID, string ImagePath, int Width, int Height) : HtmlPart;
    public record HtmlLink(string Label, string ActionUrl): HtmlPartWithStyles;
    public record HtmlPartCollection(params HtmlPart[] Parts) : HtmlPart;
    public record HtmlTag(string Tag, HtmlPart Content) : HtmlPartWithStyles;
    public record HtmlText(string Text) : HtmlPart;
    public record HtmlNothing(): HtmlPart;
    public record HtmlClosedTag(string Tag): HtmlPartWithStyles;
    public record HtmlUnorderedList(params HtmlListItem[] Items): HtmlPartWithStyles;
    public record HtmlOrderedList(params HtmlListItem[] Items): HtmlPartWithStyles;
    public record HtmlListItem(HtmlPart Content): HtmlPartWithStyles;
}
