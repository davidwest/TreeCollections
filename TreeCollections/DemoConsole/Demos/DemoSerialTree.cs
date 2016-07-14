
using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSerialTree
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public static void Start()
        {
            var dataRoot = GetRoot();
            Debug.WriteLine("*** Original ***");
            Display(dataRoot);
            
            var json = JsonConvert.SerializeObject(dataRoot);
            Debug.WriteLine("*** Serialized ***\n");
            Debug.WriteLine(json + "\n\n");
            File.WriteAllText(Path.Combine(DesktopPath, "testcategories.json"), json);

            dataRoot = JsonConvert.DeserializeObject<CategoryDataNode>(json);
            Debug.WriteLine("*** De-serialized ***");
            Display(dataRoot);

            var root = new SimpleMutableCategoryNode(new CategoryItem(dataRoot.CategoryId, dataRoot.Name));
            root.Build(dataRoot, d => new CategoryItem(d.CategoryId, d.Name), n => !n.Name.StartsWith("c", StringComparison.OrdinalIgnoreCase));
            Debug.WriteLine("*** Proper Tree (with some ignored) ***");
            Display(root);

            dataRoot = new CategoryDataNode(root.Id, root.Name);
            root.CopyTo(dataRoot, (n, d) =>
            {
                d.CategoryId = n.Item.CategoryId;
                d.Name = n.Item.Name;
            });

            Debug.WriteLine("*** Converted back to Serializable ***");
            Display(dataRoot);
        }

        private static void Display(CategoryDataNode root)
        {
            Debug.WriteLine("\n" + root.ToString(n => $"{n.CategoryId} {n.Name}") + "\n");
        }

        private static void Display(SimpleMutableCategoryNode root)
        {
            Debug.WriteLine("\n" + root.ToString(n => $"{n.Id} {n.Item.Name} [{n.HierarchyId.ToString(".")}] {(n.Error != IdentityError.None ? n.Error.Normalize().ToString() : string.Empty)}") + "\n");
        }

        private static CategoryDataNode GetRoot()
        {
            var root =
                new CategoryDataNode(1, "Root", 
                    new CategoryDataNode(2, "Nouns", 
                        new CategoryDataNode(3, "People", 
                            new CategoryDataNode(4, "Celebrities"), 
                            new CategoryDataNode(5, "Bad Guys")), 
                        new CategoryDataNode(6, "Places", 
                            new CategoryDataNode(13, "Local"), 
                            new CategoryDataNode(14, "National"), 
                            new CategoryDataNode(15, "International")), 
                        new CategoryDataNode(7, "Things", 
                            new CategoryDataNode(16, "Concrete", 
                                new CategoryDataNode(21, "Everyday Objects"), 
                                new CategoryDataNode(22, "Tools")), 
                            new CategoryDataNode(17, "Abstract", 
                                new CategoryDataNode(4, "Philosophies"), 
                                new CategoryDataNode(19, "Math")))), 
                    new CategoryDataNode(8, "Verbs", 
                        new CategoryDataNode(10, "Active"), 
                        new CategoryDataNode(11, "Passive")), 
                    new CategoryDataNode(9, "Adjectives", 
                        new CategoryDataNode(12, "Sensory", 
                            new CategoryDataNode(23, "Colors"), 
                            new CategoryDataNode(1, "Smells")), 
                        new CategoryDataNode(20, "Sizes")));

            return root;
        }
    }
}
