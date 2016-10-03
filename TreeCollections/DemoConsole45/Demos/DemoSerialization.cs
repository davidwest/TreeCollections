
using System.IO;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSerialization
    {
        private const string OutputFolderName = @"c:\TreeCollectionDemos";
        private const string JsonFileName = "DemoSerialization.json";
        private const string HtmlFileName = "DemoSerialization.html";

        public static void Start()
        {
            InitializeOutputFolder();

            var root = new CategoryTreeLookup("Source2").GetAdvancedMutableCategoryTree();

            DemoHtmlSerialization(root);
            DemoJsonSerialization(root);
        }

        private static void DemoHtmlSerialization(AdvancedMutableCategoryNode root)
        {
            var def = new HtmlBuildDefinition<AdvancedMutableCategoryNode>
            {
                GetItemPreHtml = n => $"{n.Id} {n.Name}"
            };

            var builder = new TreeHtmlBuilder<AdvancedMutableCategoryNode>(def);

            var html = builder.ToHtml(root);
            //var html = builder.ToHtml(root, 2, false);

            File.WriteAllText(Path.Combine(OutputFolderName, HtmlFileName), html);
        }

        private static void DemoJsonSerialization(AdvancedMutableCategoryNode root)
        {
            var builder = new TreeJsonBuilder<AdvancedMutableCategoryNode>();
            var json = builder.ToJson(root);
            File.WriteAllText(Path.Combine(OutputFolderName, JsonFileName), json);
        }

        private static void InitializeOutputFolder()
        {
            if (!Directory.Exists(OutputFolderName))
            {
                Directory.CreateDirectory(OutputFolderName);
            }
        }
    }
}
