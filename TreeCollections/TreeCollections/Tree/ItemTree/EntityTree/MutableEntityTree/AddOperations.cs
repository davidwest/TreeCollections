
using System;

namespace TreeCollections
{
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
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


        public virtual TNode AddAtAdjacentPosition(TItem item, Adjacency adjacency)
        {
            if (IsRoot)
            {
                throw new InvalidOperationException("Cannot insert at root level");
            }

            var insertIndex = OrderIndex + (adjacency == Adjacency.Before ? 0 : 1);

            return Parent.AddChild(item, insertIndex);
        }

        public TNode AddAtPositionBefore(TItem item) => AddAtAdjacentPosition(item, Adjacency.Before);
        public TNode AddAtPositionAfter(TItem item) => AddAtAdjacentPosition(item, Adjacency.After);
    }
}
