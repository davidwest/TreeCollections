
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeCollections
{
    public abstract class SerialTreeNode<TNode> where TNode : SerialTreeNode<TNode>, new()
    {
        private readonly TNode _this;

        protected SerialTreeNode(params TNode[] children)
        {
            Children = children ?? new TNode[0];
            _this = (TNode)this;
        }

        public TNode[] Children { get; set; }

        public IEnumerable<TNode> Where(Func<TNode, bool> isMatch)
        {
            return FindRecursive(_this, isMatch);
        }
        
        public IEnumerable<TNode> SelectAll()
        {
            return Where(n => true);
        }
        
        public TNode FirstOrDefault(Func<TNode, bool> isMatch)
        {
            return Where(isMatch).FirstOrDefault();
        }

        public void ForEach(Action<TNode, int> doIt)
        {
            ForEach(_this, 0, doIt);
        }

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
