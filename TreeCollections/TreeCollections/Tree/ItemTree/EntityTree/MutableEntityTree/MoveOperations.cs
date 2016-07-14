
using System;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        public virtual void MoveToParent(TId parentId, int? insertIndex = null)
        {
            var newParent = Root[parentId];

            var isAlreadyHere = insertIndex.HasValue
                                    ? newParent._children.IndexOf(This) == insertIndex.Value
                                    : newParent._children.LastOrDefault()?.Equals(This) ?? false;

            if (isAlreadyHere)
            {
                return;
            }

            if (newParent.Equals(this) || IsAncestorOf(newParent))
            {
                throw new InvalidOperationException("Cannot move to self or a descendant");
            }

            Detach();
            newParent.AttachChildOnMove(This, insertIndex);
        }


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

        public void MoveToPositionBefore(TId targetId) => MoveToAdjacentPosition(targetId, Adjacency.Before);
        public void MoveToPositionAfter(TId targetId) => MoveToAdjacentPosition(targetId, Adjacency.After);
    }
}
