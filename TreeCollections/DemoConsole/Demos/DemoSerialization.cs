
using System;
using System.IO;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSerialization
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public static void Start()
        {
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

            File.WriteAllText(Path.Combine(DesktopPath, "categoryhtml.html"), html);
        }

        private static void DemoJsonSerialization(AdvancedMutableCategoryNode root)
        {
            var builder = new TreeJsonBuilder<AdvancedMutableCategoryNode>();
            var json = builder.ToJson(root);
            File.WriteAllText(Path.Combine(DesktopPath, "categoryjson.json"), json);
        }
    }
}
