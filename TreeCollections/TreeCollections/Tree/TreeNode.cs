using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TreeCollections
{
    public abstract partial class TreeNode<TNode> : IEnumerable<TNode>, ITreeNode
            where TNode : TreeNode<TNode>
    {
        private readonly Lazy<HierarchyPosition> _hierarchyId;
        
        protected TreeNode(TNode parent, IReadOnlyList<TNode> children)
        {
            This = (TNode) this;
            Parent = parent;
            Root = parent?.Root ?? This;
            Level = parent?.Level + 1 ?? 0;
            Children = children;
            
            _hierarchyId = new Lazy<HierarchyPosition>(GetHierarchyId, LazyThreadSafetyMode.PublicationOnly);
        }

        public TNode Root { get; internal set; }
        public int Level { get; internal set; }
        public TNode Parent { get; internal set; }
        public TNode NextSibling { get; internal set; }
        public TNode PreviousSibling { get; internal set; }
        public IReadOnlyList<TNode> Children { get; }
        
        public HierarchyPosition HierarchyId => IsReadOnly ? _hierarchyId.Value : GetHierarchyId();

        public abstract bool IsReadOnly { get; }

        public IEnumerable<TNode> PreOrder(int? maxRelativeDepth = null)
        {
            return new EnumeratorHost<TNode>(new PreOrderEnumerator<TNode>(This, maxRelativeDepth));
        }
        
        public IEnumerable<TNode> PreOrder(Func<TNode, bool> allowNext, int? maxRelativeDepth = null)
        {
            return new EnumeratorHost<TNode>(new PreOrderFilteringEnumerator<TNode>(This, allowNext, maxRelativeDepth));
        }

        public IEnumerable<TNode> LevelOrder(int? maxRelativeDepth = null)
        {
            return new EnumeratorHost<TNode>(new LevelOrderEnumerator<TNode>(This, maxRelativeDepth));
        }
        
        public IEnumerable<TNode> EnumerateWith(IEnumerator<TNode> enumerator)
        {
            return new EnumeratorHost<TNode>(enumerator);
        } 

        public IEnumerator<TNode> GetEnumerator()
        {
            return new PreOrderEnumerator<TNode>(This);

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected TNode This { get; }

        protected void SetChildrenSiblingReferences()
        {
            for (var i = 0; i != Children.Count; i++)
            {
                var cur = Children[i];
                cur.PreviousSibling = i != 0 ? Children[i - 1] : null;
                cur.NextSibling = i != Children.Count - 1 ? Children[i + 1] : null;
            }
        }

        private HierarchyPosition GetHierarchyId()
        {
            var ordinals =
                SelectPathDownward()
                .Select(n => n.Parent?.Children
                            .Select((child, i) => new { child, index = i + 1 })
                            .First(ni => ni.child.Equals(n)).index ?? 0);

            return new HierarchyPosition(ordinals.ToArray());
        }
    }
}
