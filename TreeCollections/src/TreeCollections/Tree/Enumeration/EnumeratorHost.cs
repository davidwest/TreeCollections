
using System.Collections;
using System.Collections.Generic;

namespace TreeCollections
{
    public class EnumeratorHost<TNode> : IEnumerable<TNode>
        where TNode : TreeNode<TNode>
    {
        private readonly IEnumerator<TNode> _enumerator;

        internal EnumeratorHost(IEnumerator<TNode> enumerator)
        {
            _enumerator = enumerator;
        }
  
        public IEnumerator<TNode> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
