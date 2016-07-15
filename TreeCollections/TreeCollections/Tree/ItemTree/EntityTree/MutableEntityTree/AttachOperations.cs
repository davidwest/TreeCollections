
using System;

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        public virtual void AttachChild(TNode node, int? insertionIndex = null)
        {
            if (node.Root.Equals(Root))
            {
                throw new InvalidOperationException("Node to manually attach must first be detached from this tree");
            }

            if (!node.IsRoot)
            {
                throw new InvalidOperationException("Node to manually attach must be a root");
            }

            if (!IsCompatible(node))
            {
                throw new InvalidOperationException("Node to manually attach must share the same entity definition instance and have identical error check options");
            }

            AttachChildOnMove(node, insertionIndex);
        }


        public virtual void AttachAtAdjacentPosition(TNode node, Adjacency adjacency)
        {
            var insertionIndex = OrderIndex + (adjacency == Adjacency.Before ? 0 : 1);

            Parent.AttachChild(node, insertionIndex);
        }

        public void AttachAtPositionBefore(TNode node) => AttachAtAdjacentPosition(node, Adjacency.Before);
        public void AttachAtPositionAfter(TNode node) => AttachAtAdjacentPosition(node, Adjacency.After);
    }
}
