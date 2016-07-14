
namespace TreeCollections.DemoConsole.Demos
{
    public class CategoryDataNode : SerialTreeNode<CategoryDataNode>
    {
        public CategoryDataNode() { }

        public CategoryDataNode(long categoryId, string name, params CategoryDataNode[] children) 
            : base(children)
        {
            CategoryId = categoryId;
            Name = name;
        }
        
        public long CategoryId { get; set; }
        public string Name { get; set; }
    }
}
