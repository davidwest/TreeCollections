using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public abstract partial class TreeNode<TNode> 
        where TNode : TreeNode<TNode>
    {
        /// <summary>
        /// Select all siblings of this node
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectSiblings()
        {
            return Parent?.Children.Where(n => !n.Equals(this)) ?? Enumerable.Empty<TNode>();
        }

        /// <summary>
        /// Select siblings before this node
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectSiblingsBefore()
        {
            return Parent?.Children.TakeWhile(n => !n.Equals(this)) ?? Enumerable.Empty<TNode>();
        }

        /// <summary>
        /// Select siblings after this node
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectSiblingsAfter()
        {
            return Parent?.Children.SkipWhile(n => !n.Equals(this)).Skip(1) ?? Enumerable.Empty<TNode>();
        }

        /// <summary>
        /// Select all descendants of this node
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectDescendants() => this.Skip(1);
        
        /// <summary>
        /// Select all leaves beneath this node (or node itself if leaf)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectLeaves() => this.Where(n => n.IsLeaf);
        
        /// <summary>
        /// Select this node and all ancestral nodes in the path up to the root
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectPathUpward()
        {
            var cur = this;
            while (cur != null)
            {
                yield return cur.This;
                cur = cur.Parent;
            }
        }

        /// <summary>
        /// Select all ancestral nodes in the path from this node's parent to the root
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectAncestorsUpward() => SelectPathUpward().Skip(1);

        /// <summary>
        /// Select this node and all ancestral nodes in the path downward starting from the root
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectPathDownward() => SelectPathUpward().Reverse();

        /// <summary>
        /// Select all ancestral nodes to this node as a path downward starting from the root
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectAncestorsDownward() => SelectAncestorsUpward().Reverse();

        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children.Count == 0;

        /// <summary>
        /// Max depth of the branch containing this node; for the root, this is the tree's max depth
        /// </summary>
        public int Depth => this.Max(n => n.Level);

        /// <summary>
        /// Determines if this node is an ancestor of the given node
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsAncestorOf(TNode other) => other.SelectAncestorsUpward().Any(n => n.Equals(this));
        
        /// <summary>
        /// Determines if this node is a descendant of the given node
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsDescendantOf(TNode other) => SelectAncestorsUpward().Any(n => n.Equals(other));

        /// <summary>
        /// Determines if this node is a sibling of the given node
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSiblingOf(TNode other) => Parent?.Equals(other.Parent) ?? false;
    }
}
