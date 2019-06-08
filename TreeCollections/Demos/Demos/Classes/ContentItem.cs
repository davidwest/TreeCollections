namespace TreeCollections.DemoConsole.Demos
{
    public class ContentItem
    {
        public ContentItem(long contentId, string name)
        {
            ContentId = contentId;
            Name = name;
        }

        public long ContentId { get; }
        public string Name { get; }
    }
}
