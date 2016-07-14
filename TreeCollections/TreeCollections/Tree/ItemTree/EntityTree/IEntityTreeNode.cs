
namespace TreeCollections
{
    public interface IEntityTreeNode<out TId, out TItem> : IItemTreeNode<TItem>
    {
        TId Id { get; }
    }
}
