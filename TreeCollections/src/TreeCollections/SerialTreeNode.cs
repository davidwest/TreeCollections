using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeCollections
{
    /// <summary>
    /// Abstract tree node supporting one way (parent to child) hierarchical relationships 
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public abstract class SerialTreeNode<TNode> where TNode : SerialTreeNode<TNode>, new()
    {
        private readonly TNode _this;

        protected SerialTreeNode(params TNode[] children)
        {
            Children = children ?? new TNode[0];
            _this = (TNode)this;
        }

        public TNode[] Children { get; set; }

        /// <summary>
        /// Filters all nodes matching a predicate starting from this node with pre-order traversal
        /// </summary>
        /// <param name="isMatch"></param>
        /// <returns></returns>
        public IEnumerable<TNode> Where(Func<TNode, bool> isMatch)
        {
            return FindRecursive(_this, isMatch).Where(x => true);
        }
        
        /// <summary>
        /// Returns sequence of all nodes starting from this node with pre-order traversal
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TNode> SelectAll()
        {
            return Where(n => true);
        }
        
        /// <summary>
        /// Returns first element of all nodes matching a predicate starting from this node with pre-order traversal
        /// </summary>
        /// <param name="isMatch"></param>
        /// <returns></returns>
        public TNode FirstOrDefault(Func<TNode, bool> isMatch)
        {
            return Where(isMatch).FirstOrDefault();
        }

        /// <summary>
        /// Perform an action on each node starting from this node with pre-order traversal
        /// </summary>
        /// <param name="doIt"></param>
        public void ForEach(Action<TNode, int> doIt)
        {
            ForEach(_this, 0, doIt);
        }

        /// <summary>
        /// Convert this node to a string representation
        /// </summary>
        /// <param name="toLineOfText"></param>
        /// <param name="indentation"></param>
        /// <returns></returns>
        public string ToString(Func<TNode, string> toLineOfText, int indentation = 5)
        {
            var builder = new StringBuilder();
            ForEach( (n,l) => builder.AppendLine($"{string.Empty.PadLeft(l * indentation)}{toLineOfText(n)}"));

            return builder.ToString();
        }

        private static IEnumerable<TNode> FindRecursive(TNode node, Func<TNode, bool> isMatch)
        {
            if (isMatch(node)) yield return node;

            foreach (var descendant in node.Children.SelectMany(child => FindRecursive(child, isMatch)))
            {
                yield return descendant;
            }
        }

        private static void ForEach(TNode cur, int relativeLevel, Action<TNode, int> doIt)
        {
            doIt(cur, relativeLevel);

            relativeLevel ++;

            foreach (var child in cur.Children)
            {
                ForEach(child, relativeLevel, doIt);
            }
        }
    }
}
