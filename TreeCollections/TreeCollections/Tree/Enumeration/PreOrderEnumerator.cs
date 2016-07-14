
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public class PreOrderEnumerator<TNode> : IEnumerator<TNode>
        where TNode : TreeNode<TNode>
    {
        private readonly TNode _rootOfIteration;
        private readonly int _maxLevel;

        internal PreOrderEnumerator(TNode rootOfIteration, int? maxRelativeDepth = null)
        {
            _rootOfIteration = rootOfIteration;
            _maxLevel = rootOfIteration.Level + maxRelativeDepth ?? int.MaxValue;
            
            Current = null;
        } 
        
        public bool MoveNext()
        {
            if (Current == null)
            {
                Current = _rootOfIteration;
                return true;
            }

            var firstChild = Current.Children.FirstOrDefault();

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
            var nextSibling = Current.NextSibling;

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
