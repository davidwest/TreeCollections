
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class TreeNode<TNode> 
        where TNode : TreeNode<TNode>
    {
        public IEnumerable<TNode> SelectSiblings()
        {
            return Parent?.Children.Where(n => !n.Equals(this)) ?? EmptyNodes();
        }

        public IEnumerable<TNode> SelectSiblingsBefore()
        {
            return Parent?.Children.TakeWhile(n => !n.Equals(this)) ?? EmptyNodes();
        }

        public IEnumerable<TNode> SelectSiblingsAfter()
        {
            return Parent?.Children.SkipWhile(n => !n.Equals(this)).Skip(1) ?? EmptyNodes();
        }

        public IEnumerable<TNode> SelectDescendants() => this.Skip(1);
        
        public IEnumerable<TNode> SelectLeaves() => this.Where(n => n.Children.Count == 0);
        
        public IEnumerable<TNode> SelectPathUpward()
        {
            var cur = this;
            while (cur != null)
            {
                yield return cur.This;
                cur = cur.Parent;
            }
        }

        public IEnumerable<TNode> SelectAncestorsUpward() => SelectPathUpward().Skip(1); 
        public IEnumerable<TNode> SelectPathDownward() => SelectPathUpward().Reverse();
        public IEnumerable<TNode> SelectAncestorsDownward() => SelectAncestorsUpward().Reverse();

        public bool IsRoot => Parent == null;

        public int Depth => this.Max(n => n.Level);

        public bool IsAncestorOf(TNode other) => other.SelectAncestorsUpward().Any(n => n.Equals(this));
        
        public bool IsDescendantOf(TNode other) => SelectAncestorsUpward().Any(n => n.Equals(other));

        public bool IsSiblingOf(TNode other) => Parent?.Equals(other.Parent) ?? false;

        private static IEnumerable<TNode> EmptyNodes()
        {
            yield break;
        }
    }
}
