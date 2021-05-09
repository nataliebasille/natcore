using System.Text;

namespace Natcore.Core.Messaging
{
    public record HtmlStyles
    {
        public string Color { get; init; }

        public string BackgroundColor { get; init; }

        public string Padding { get; init; }

        public string Margin { get; init; }

        public string Border { get; init; }

        public string BorderRadius { get; init; }

        public string Width { get; init; }

        public string Height { get; init; }

        public string FontFamily { get; init; }

        public string FontSize { get; init; }

        public string FontWeight { get; init; }

        public string TextDecoration { get; init; }

        public string CreateStyles()
        {
            StringBuilder builder = new StringBuilder();

            Add(builder, "color", Color);
            Add(builder, "background-color", BackgroundColor);
            Add(builder, "padding", Padding);
            Add(builder, "margin", Margin);
            Add(builder, "border", Border);
            Add(builder, "border-radius", BorderRadius);
            Add(builder, "height", Height);
            Add(builder, "width", Width);
            Add(builder, "font-family", FontFamily);
            Add(builder, "font-size", FontSize);
            Add(builder, "font-weight", FontWeight);
            Add(builder, "text-decoration", TextDecoration);

            return builder.ToString();
        }

        private static void Add(StringBuilder builder, string property, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                builder.Append($"{property}:{value}; ");
            }
        }
    }
}
