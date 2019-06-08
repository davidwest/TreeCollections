using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable UnusedTypeParameter

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Detach this node from the tree
        /// </summary>
        public virtual void Detach()
        {
            if (IsRoot)
            {
                throw new InvalidOperationException("Cannot detach a root");
            }

            OnNodeDetaching();

            UpdateErrorsBeforeDetachingThis();

            var oldParent = Parent;

            Root = This;
            Parent = null;
            Level = 0;
            PreviousSibling = null;
            NextSibling = null;

            oldParent.ChildrenList.Remove(This);

            oldParent.SetChildrenSiblingReferences();

            foreach (var n in SelectDescendants())
            {
                n.Level = n.Parent.Level + 1;
                n.Root = This;
                n.TreeIdMap = TreeIdMap;
            }

            OnNodeDetached(oldParent);
        }

        /// <summary>
        /// Detach nodes from the tree that satisfy the specified predicate.
        /// Nodes in scope of this operation include this node and all descendants.
        /// Returns sequence of detached nodes.
        /// </summary>
        /// <param name="satisfiesCondition">Filtering predicate</param>
        public virtual IEnumerable<TNode> DetachWhere(Func<TNode, bool> satisfiesCondition)
        {
            var nodesToRemove = this.Where(satisfiesCondition).OrderBy(n => n.Level).ToList();

            while (nodesToRemove.Count > 0)
            {
                var cur = nodesToRemove[0];

                if (!cur.IsRoot)
                {
                    cur.Detach();
                    yield return cur;
                }

                nodesToRemove.Remove(cur);
            }
        }

        /// <summary>
        /// Detach this node's children from the tree. Returns sequence of detached nodes.
        /// </summary>
        public virtual IEnumerable<TNode> DetachChildren()
        {
            while (ChildrenList.Count > 0)
            {
                var child = ChildrenList[0];
                child.Detach();
                yield return child;
            }
        }
    }
}
