using System;

namespace TreeCollections
{
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Add new new entity node as child of this node
        /// </summary>
        /// <param name="item">Entity to add</param>
        /// <param name="insertIndex">Child position at which to insert</param>
        /// <returns></returns>
        public virtual TNode AddChild(TItem item, int? insertIndex = null)
        {
            if (!OnAddCanProceed())
            {
                return null;
            }

            var newNode = Create(item, This);

            AttachChildOnAdd(newNode, insertIndex);

            return newNode;
        }

        /// <summary>
        /// Add new entity node as sibling adjacent to this node
        /// </summary>
        /// <param name="item">Entity to add</param>
        /// <param name="adjacency">Specifies which side to place the node</param>
        /// <returns></returns>
        public virtual TNode AddAtAdjacentPosition(TItem item, Adjacency adjacency)
        {
            if (IsRoot)
            {
                throw new InvalidOperationException("Cannot insert at root level");
            }

            var insertIndex = OrderIndex + (adjacency == Adjacency.Before ? 0 : 1);

            return Parent.AddChild(item, insertIndex);
        }

        /// <summary>
        /// Add new entity node as sibling before this node
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public TNode AddAtPositionBefore(TItem item) => AddAtAdjacentPosition(item, Adjacency.Before);

        /// <summary>
        /// Add new entity node as sibling after this node
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public TNode AddAtPositionAfter(TItem item) => AddAtAdjacentPosition(item, Adjacency.After);
    }
}
