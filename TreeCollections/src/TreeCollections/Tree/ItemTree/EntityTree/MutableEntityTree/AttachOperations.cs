using System;
// ReSharper disable UnusedTypeParameter

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Attach existing entity node as child of this node
        /// </summary>
        /// <param name="node">Node to attach</param>
        /// <param name="insertionIndex">Child position at which to insert</param>
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

            node.OnNodeReparenting(This);

            AttachChildOnMove(node, insertionIndex);
        }

        /// <summary>
        /// Attach existing entity node as sibling adjacent to this node
        /// </summary>
        /// <param name="node">Node to attach</param>
        /// <param name="adjacency">Specifies which side to place the node</param>
        public virtual void AttachAtAdjacentPosition(TNode node, Adjacency adjacency)
        {
            var insertionIndex = OrderIndex + (adjacency == Adjacency.Before ? 0 : 1);

            Parent.AttachChild(node, insertionIndex);
        }

        /// <summary>
        /// Attach existing entity node as sibling before this node
        /// </summary>
        /// <param name="node"></param>
        public void AttachAtPositionBefore(TNode node) => AttachAtAdjacentPosition(node, Adjacency.Before);

        /// <summary>
        /// Attach existing entity node as sibling after this node
        /// </summary>
        /// <param name="node"></param>
        public void AttachAtPositionAfter(TNode node) => AttachAtAdjacentPosition(node, Adjacency.After);
    }
}
