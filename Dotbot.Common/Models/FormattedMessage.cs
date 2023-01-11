using System.Drawing;

namespace Dotbot.Common.Models;

public class FormattedMessage
{
    public string Title { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
    public string ImageUrl { get; init; }
    public string Description { get; set; }
    public Color? Color { get; set; }
    public List<Field> Fields { get; set; } = new();

    //TODO: Footer, Author, Header
    
    public class Field
    {
        public string Name { get; set; }
        public string? Value { get; set; }

        public bool Inline { get; set; } = false;
    }
    
}