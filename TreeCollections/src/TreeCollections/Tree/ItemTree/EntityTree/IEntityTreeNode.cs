namespace TreeCollections
{
    /// <summary>
    /// Represents a tree node whose payload item has a unique and separate existence from all other items in the same tree.
    /// Such a node is solely distinguishable from other nodes by its Id which will be that of its contained item.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public interface IEntityTreeNode<out TId, out TItem> : IItemTreeNode<TItem>
    {
        TId Id { get; }
    }
}
