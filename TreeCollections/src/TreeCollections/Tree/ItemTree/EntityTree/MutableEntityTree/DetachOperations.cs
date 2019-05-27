using System;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
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

            oldParent._children.Remove(This);

            oldParent.SetChildrenSiblingReferences();

            foreach (var n in SelectDescendants())
            {
                n.Level = n.Parent.Level + 1;
                n.Root = This;
                n.TreeIdMap = TreeIdMap;
            }

            OnNodeDetached(oldParent);
        }


        public virtual void DetachWhere(Func<TNode, bool> satisfiesCondition)
        {
            var nodesToRemove = this.Where(satisfiesCondition).OrderBy(n => n.Level).ToList();

            while (nodesToRemove.Count > 0)
            {
                var cur = nodesToRemove[0];

                if (!cur.IsRoot)
                {
                    cur.Detach();
                }

                nodesToRemove.Remove(cur);
            }
        }


        public virtual void DetachChildren()
        {
            while (_children.Count > 0)
            {
                var child = _children[0];
                child.Detach();
            }
        }
    }
}
