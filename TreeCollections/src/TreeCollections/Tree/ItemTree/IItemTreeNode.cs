namespace TreeCollections
{
    /// <summary>
    /// Represents a tree node containing a payload item
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface IItemTreeNode<out TItem> : ITreeNode
    {
        TItem Item { get; }
    }
}
