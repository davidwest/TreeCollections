using System.Collections;
using System.Collections.Generic;

namespace TreeCollections
{
    /// <summary>
    /// Enumerator for level-order (breadth-first) traversal with optional max depth of traversal
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public class LevelOrderEnumerator<TNode> : IEnumerator<TNode>
        where TNode : TreeNode<TNode>
    {
        private readonly TNode _rootOfIteration;
        private readonly int _maxDepth;
        private readonly Queue<TNode> _queue;
        private int _currentDepth;
        private int _currentGenerationCount;
        private int _nextGenerationCount;
        
        internal LevelOrderEnumerator(TNode rootOfIteration, int? maxRelativeDepth = null)
        {
            _rootOfIteration = rootOfIteration;
            _currentDepth = 0;
            _queue = new Queue<TNode>();
            _currentGenerationCount = 1;
            _nextGenerationCount = 0;
            _maxDepth = maxRelativeDepth ?? int.MaxValue;

            Current = null;
        }

        public bool MoveNext()
        {
            if (Current == null)
            {
                Current = _rootOfIteration;
                ProcessCurrent();
                return true;
            }

            if (_queue.Count == 0)
            {
                return false;
            }

            if (_currentGenerationCount == 0)
            {
                SwapGeneration();
            }

            Current = _queue.Dequeue();
            ProcessCurrent();

            return true;
        }

        private void ProcessCurrent()
        {
            _currentGenerationCount--;

            if (_currentDepth >= _maxDepth) return;

            foreach (var child in Current.Children)
            {
                _nextGenerationCount++;
                _queue.Enqueue(child);
            }
        }

        private void SwapGeneration()
        {
            _currentDepth++;
            _currentGenerationCount = _nextGenerationCount;
            _nextGenerationCount = 0;
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