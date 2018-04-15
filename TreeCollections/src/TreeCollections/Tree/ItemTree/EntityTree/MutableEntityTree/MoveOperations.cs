using System;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {        
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
                    var effectiveInsertIndex = insertIndex.Value > 0 ? insertIndex.Value : 0;

                    var adjacency = effectiveInsertIndex < OrderIndex ? Adjacency.Before : Adjacency.After;

                    MoveToSiblingAdjacentPosition(Parent.Children[effectiveInsertIndex], adjacency);
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


        public virtual void IncrementSiblingPosition()
        {
            if (NextSibling == null) return;

            MoveToSiblingAdjacentPosition(NextSibling, Adjacency.After);
        }


        public virtual void DecrementSiblingPosition()
        {
            if (PreviousSibling == null) return;

            MoveToSiblingAdjacentPosition(PreviousSibling, Adjacency.Before);
        }
    }
}
