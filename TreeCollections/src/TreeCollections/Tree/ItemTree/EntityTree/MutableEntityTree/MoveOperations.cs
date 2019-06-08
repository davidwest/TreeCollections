using System;
using System.Linq;
// ReSharper disable UnusedTypeParameter

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Move this node from its current position to new parent in the same tree
        /// </summary>
        /// <param name="parentId">Target parent entity Id</param>
        /// <param name="insertIndex">Child position at which to insert</param>
        public virtual void MoveToParent(TId parentId, int? insertIndex = null)
        {
            var targetParent = Root[parentId];

            if (targetParent.Equals(Parent))
            {
                if (!insertIndex.HasValue || insertIndex.Value > Parent.Children.Count - 1)
                {
                    MoveToSiblingAdjacentPosition(Parent.Children.Last(), Adjacency.After);
                }
                else
                {
                    MoveToSiblingAdjacentPosition(Parent.Children[insertIndex.Value], Adjacency.After);
                }

                return;
            }
            
            if (targetParent.Equals(this) || IsAncestorOf(targetParent))
            {
                throw new InvalidOperationException("Cannot move to self or a descendant");
            }

            OnNodeReparenting(targetParent);

            Detach();
            targetParent.AttachChildOnMove(This, insertIndex);
        }

        /// <summary>
        /// Move this node from its current position to a position adjacent to a specified node in the same tree.
        /// </summary>
        /// <param name="targetId">Target entity Id</param>
        /// <param name="adjacency">Specifies which side to place the node</param>
        public virtual void MoveToAdjacentPosition(TId targetId, Adjacency adjacency)
        {
            var targetNode = Root[targetId];

            if (targetNode.Equals(this))
            {
                // already here!
                return;
            }

            if (targetNode.IsRoot)
            {
                throw new InvalidOperationException("Cannot move to root level");
            }

            if (IsAncestorOf(targetNode))
            {
                throw new InvalidOperationException("Cannot move to a descendant");
            }

            if (IsSiblingOf(targetNode))
            {
                MoveToSiblingAdjacentPosition(targetNode, adjacency);
            }
            else
            {
                MoveToNonSiblingAdjacentPosition(targetNode, adjacency);
            }
        }

        /// <summary>
        /// Move this node from its current position to the adjacent position before a specified node in the same tree.
        /// </summary>
        /// <param name="targetId">Target entity Id</param>
        public void MoveToPositionBefore(TId targetId) => MoveToAdjacentPosition(targetId, Adjacency.Before);

        /// <summary>
        /// Move this node from its current position to the adjacent position after a specified node in the same tree.
        /// </summary>
        /// <param name="targetId"></param>
        public void MoveToPositionAfter(TId targetId) => MoveToAdjacentPosition(targetId, Adjacency.After);

        /// <summary>
        /// Move this node "forward" to the next sibling position.  Has no effect if no siblings exist.
        /// </summary>
        public virtual void IncrementSiblingPosition()
        {
            if (NextSibling == null) return;

            MoveToSiblingAdjacentPosition(NextSibling, Adjacency.After);
        }

        /// <summary>
        /// Move this node "backward" to the previous sibling position.  Has no effect if no siblings exist.
        /// </summary>
        public virtual void DecrementSiblingPosition()
        {
            if (PreviousSibling == null) return;

            MoveToSiblingAdjacentPosition(PreviousSibling, Adjacency.Before);
        }
    }
}
