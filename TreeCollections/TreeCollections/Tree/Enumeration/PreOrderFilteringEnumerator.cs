using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public class PreOrderFilteringEnumerator<TNode> : IEnumerator<TNode>
        where TNode : TreeNode<TNode>
    {
        private readonly TNode _rootOfIteration;
        private readonly int _maxLevel;
        private readonly Func<TNode, bool> _allowNext; 

        internal PreOrderFilteringEnumerator(TNode rootOfIteration, 
                                             Func<TNode, bool> allowNext, 
                                             int? maxRelativeDepth = null)
        {
            _rootOfIteration = rootOfIteration;
            _maxLevel = rootOfIteration.Level + maxRelativeDepth ?? int.MaxValue;
            _allowNext = allowNext;
            
            Current = null;
        } 
        
        public bool MoveNext()
        {
            if (Current == null)
            {
                Current = _rootOfIteration;
                return true;
            }

            var firstChild = Current.Children.FirstOrDefault(_allowNext);

            if (firstChild != null && firstChild.Level <= _maxLevel)
            {
                Current = firstChild;
                return true;
            }

            if (Current.Equals(_rootOfIteration))
            {
                return false;
            }

            var node = Current;

            var nextSibling = Current.SelectSiblingsAfter().FirstOrDefault(_allowNext);

            while (nextSibling == null)
            {
                node = node.Parent;

                if (node.Equals(_rootOfIteration))
                {
                    return false;
                }

                nextSibling = node.NextSibling;
            }
            
            Current = nextSibling;
            return true;
        }

        public TNode Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose()
        { }

        public void Reset()
        {
            Current = null;
        }
    }
}
