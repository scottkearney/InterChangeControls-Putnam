namespace Akumina.WebParts.ContentBlock
{
    public class ContentBlockModel
    {
        public string UniqueId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public string ColorTheme { get; set; }
        public string WebPartTitle { get; set; }
        public string WebPartIcon { get; set; }
        public bool ShowHeader { get; set; }
    }

    public enum Themes
    {
        Alert,
        Red,
        Yellow,
        Orange,
        Green,
        Blue,
        Bluegreen,
        Violet,
        Gray,
        Black,
        White
    }
}