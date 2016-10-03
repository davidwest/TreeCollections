

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Diagnostics.Debug;

namespace TreeCollections.DemoConsole.Demos
{
    public static class DemoSimpleMutableTree
    {
        private const string OutputFolderName = @"c:\TreeCollectionDemos";
        private const string FileName = "DemoSimpleMutableTree.json";

        public static void Start()
        {
            var root = new CategoryTreeLookup("Source1").GetSimpleMutableCategoryTree();

            var sw = new Stopwatch();
            sw.Start();
            Start(root);
            sw.Stop();
            Console.WriteLine($"\n*** Post data-loaded elapsed time: {sw.Elapsed} ***");
        }

        private static void Start(SimpleMutableCategoryNode root)
        {
            Display(root);
            //DemoToJson(root);

            DemoBasicOperations(root);
            //Display(root.PreOrder(n => !n.Name.StartsWith("u", StringComparison.OrdinalIgnoreCase)));

            //DemoDetachAttachOperations(root);

            //DemoRemoveWithCondition(root);

            DemoCopy(root);
            DemoMapCopy(root);
            DemoCompress(root);
            DemoMapCompress(root);
            DemoMapCompressSearchDepthLimited(root);
            DemoMapCompressRenderDepthLimited(root);
        }


        private static void DemoToJson(SimpleMutableCategoryNode root)
        {
            var builder = new TreeJsonBuilder<SimpleMutableCategoryNode>(n => new Dictionary<string, string>
            {
                {"id", n.Id.ToString() },
                {"name",  n.Name},
                {"hierarchyId", n.HierarchyId.ToString("/") }
            }, "children");

            var json = builder.ToJson(root, 2);

            File.WriteAllText(Path.Combine(OutputFolderName, FileName), json);
        }

        private static void DemoRemoveWithCondition(SimpleMutableCategoryNode root)
        {
            root.DetachWhere(n => n.Error != IdentityError.None);
            Display(root);
        }

        private static void DemoCopy(SimpleMutableCategoryNode root)
        {
            var newRoot = new SimpleMutableCategoryNode(root.Item);
            root.CopyTo(newRoot);

            WriteLine("\n\n*** COPIED ***");
            Display(newRoot);
        }

        private static void DemoMapCopy(SimpleMutableCategoryNode root)
        {
            var newRoot = new ReadOnlyCategoryNode(new DualStateCategoryItem(root.Item));
            root.MapCopyTo(newRoot, node => new DualStateCategoryItem(node.Item));

            WriteLine("\n\n*** MAP-COPIED ***");
            Display(newRoot);
        }

        private static void DemoCompress(SimpleMutableCategoryNode root)
        {
            var newRoot = new SimpleMutableCategoryNode(root.Item);
            root.CompressTo(newRoot, n => n.Item.Name.StartsWith("m", StringComparison.OrdinalIgnoreCase));

            WriteLine("\n\n*** COMPRESSED ***");
            Display(newRoot);
        }

        private static void DemoMapCompress(SimpleMutableCategoryNode root)
        {
            var newRoot = new ReadOnlyCategoryNode(new DualStateCategoryItem(root.Item));
            root.MapCompressTo(newRoot,
                               n => n.Error != IdentityError.None,
                               n => new DualStateCategoryItem(n.Item));

            WriteLine("\n\n*** MAP-COMPRESSED ***");
            Display(newRoot);
        }
        
        private static void DemoCompressSearchDepthLimited(SimpleMutableCategoryNode root)
        {
            var newRoot = new SimpleMutableCategoryNode(root.Item);
            root.CompressTo(newRoot, 
                            n => n.Item.Name.StartsWith("m", StringComparison.OrdinalIgnoreCase), 
                            2);

            WriteLine("\n\n*** COMPRESSED (SEARCH TO LEVEL 2) ***");
            Display(newRoot);
        }

        private static void DemoMapCompressSearchDepthLimited(SimpleMutableCategoryNode root)
        {
            var newRoot = new ReadOnlyCategoryNode(new DualStateCategoryItem(root.Item));
            root.MapCompressTo(newRoot,
                               n => n.Item.Name.StartsWith("c", StringComparison.OrdinalIgnoreCase),
                               n => new DualStateCategoryItem(n.Item),
                               2);

            WriteLine("\n\n*** MAP-COMPRESSED (SEARCH TO LEVEL 2) ***");
            Display(newRoot);
        }

        private static void DemoMapCompressRenderDepthLimited(SimpleMutableCategoryNode root)
        {
            var newRoot = new ReadOnlyCategoryNode(new DualStateCategoryItem(root.Item));
            root.MapCompressTo(newRoot,
                               n => n.Item.Name.StartsWith("c", StringComparison.OrdinalIgnoreCase),
                               n => new DualStateCategoryItem(n.Item),
                               null, 2);

            WriteLine("\n\n*** MAP-COMPRESSED (RENDER TO LEVEL 2) ***");
            Display(newRoot);
        }


        private static void DemoBasicOperations(SimpleMutableCategoryNode root)
        {
            root.ForEach(n => n.OrderChildrenDescending(c => c.Name));
            Display(root);

            root[251].Detach();
            Display(root);

            root[248].Detach();
            Display(root);

            root[4001].Detach();
            Display(root);

            WriteLine(ToString(root[249].PreviousSibling));
            WriteLine(ToString(root[249].NextSibling));
            WriteLine(ToString(root[247].PreviousSibling));
            WriteLine(ToString(root[247].NextSibling));

            root[249].AddAtPositionBefore(new CategoryItem(832, "Chemistry Software"));
            Display(root);
            
            root[247].AddAtPositionAfter(new CategoryItem(833, "Atmospheric Software"));
            Display(root);

            root[246].OrderChildrenAscending(c => c.Name);
            Display(root);

            root[832].AddChild(new CategoryItem(834, "BioChemistry Software"));
            Display(root);

            root[832].AddChild(new CategoryItem(835, "Molecular Software"), 0);
            Display(root);

            root[832].AddChild(new CategoryItem(836, "Analysis Utilities"), 1);
            Display(root);

            root[832].AddChild(new CategoryItem(837, "Pharmaceutical Manufacturing"), 998877);
            Display(root);

            root[832].AddChild(new CategoryItem(246, "Measurement Tools"));
            Display(root);

            root[263].AddAtPositionBefore(new CategoryItem(838, "text Editors"));
            Display(root);

            root[243].AddAtPositionAfter(new CategoryItem(243, "Count Chocula"));
            Display(root);

            // will cause cancellation
            root[243].AddChild(new CategoryItem(999, "Kill da Wabbit"));
            Display(root);

            root[834].AddChild(new CategoryItem(238, "Let's Do Lunch"));
            Display(root);

            root[2000].MoveToParent(58);
            Display(root);

            root[838].MoveToAdjacentPosition(197, Adjacency.After);
            Display(root);

            root[246].DetachChildren();
            Display(root);

            // should do effectively nothing
            root[335].MoveToAdjacentPosition(325, Adjacency.Before);
            Display(root);

            // should do effectively nothing
            root[335].MoveToAdjacentPosition(192, Adjacency.After);
            Display(root);

            root[335].MoveToAdjacentPosition(325, Adjacency.After);
            Display(root);

            root[140].MoveToAdjacentPosition(197, Adjacency.Before);
            Display(root);

            root[253].Rename("math softwarE");
            Display(root);

            root[172].Rename("Statistical software");
            Display(root);
        }

        private static void DemoDetachAttachOperations(SimpleMutableCategoryNode root)
        {
            //// -- scenario 1 --
            //var scienceSoftware = root[246];

            //scienceSoftware.Detach();
            //root[240].AttachAtPositionAfter(scienceSoftware);
            //Display(root);

            //scienceSoftware.Detach();
            //root[141].AttachChild(scienceSoftware);
            //Display(root);


            // -- scenario 2 --
            var cat = root[336];

            cat.AddChild(new CategoryItem(326, "Shazam!"));
            Display(root);

            cat.Detach();
            Display(root);
            Display(cat);

            root[327].AttachAtPositionAfter(cat);
            Display(root);


            //// -- scenario 3 --
            //root[336].AddChild(new CategoryItem(326, "Shazam!"));
            //Display(root);

            //var software = root[1000];

            //software.Detach();
            //Display(root);
            //Display(software);

            //root[238].AttachAtPositionAfter(software);
            //Display(root);


            //// -- scenario 3 --
            //root[251].AddChild(new CategoryItem(1000, "Shazam!"));
            //Display(root);

            //var scienceSoftware = root[246];
            //scienceSoftware.Detach();
            //Display(root);
            //Display(scienceSoftware);

            //root[174].AttachChild(scienceSoftware);
            //Display(root);


            //// -- scenario 3 --
            //root[251].AddChild(new CategoryItem(1000, "Shazam!"));
            //Display(root);

            //var scienceSoftware = root[246];
            //scienceSoftware.Detach();
            //Display(root);
            //Display(scienceSoftware);

            //root[194].AttachChild(scienceSoftware);
            //Display(root);


            //// -- scenario 4 --
            //var cat = root[336];
            //cat.AddChild(new CategoryItem(951, "web BrowSers"));
            //Display(root);

            //cat.Detach();
            //Display(root);
            //Display(cat);

            //cat[3].Rename("Web Browserz");
            //Display(root);
            //Display(cat);

            //cat[3].AttachAtPositionBefore(root);
            //Display(cat);
        }

        private static void Display(IEnumerable<SimpleMutableCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(ToString));
        }

        private static void Display(IEnumerable<ReadOnlyCategoryNode> root)
        {
            WriteLine("\n" + root.ToString(n => ToString(n) + $"     ({n.HierarchyCount})"));
        }
        
        private static string ToString<TNode, TValue>(EntityTreeNode<TNode, long, TValue> n)
            where TNode : EntityTreeNode<TNode, long, TValue>
            where TValue : CategoryItem
        {
            if (n == null) return "<null>";

            var error = n.Error.Normalize();

            var errorStr = error != IdentityError.None ? $"[** {error} **]" : "";
            return $"{n.Id} {n.Item.Name} [{n.HierarchyId.ToString("/")}] {errorStr}";
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
