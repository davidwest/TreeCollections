namespace TreeCollections
{
    /// <summary>
    /// Represents a tree node with a unique hierarchy identity
    /// </summary>
    public interface ITreeNode
    {
        HierarchyPosition HierarchyId { get; }
    }
}
