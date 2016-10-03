
namespace TreeCollections
{
    public interface IItemTreeNode<out TItem> : ITreeNode
    {
        TItem Item { get; }
    }
}
