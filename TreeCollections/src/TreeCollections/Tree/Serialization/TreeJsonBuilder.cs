using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeCollections
{
    /// <summary>
    /// Builder for representing a tree as JSON
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public class TreeJsonBuilder<TNode> where TNode : TreeNode<TNode>
    {
        private readonly Func<TNode, Dictionary<string, string>> _toProperties;
        private readonly string _childrenPropertyName;

        private Func<TNode, bool> _allowNext;
        private int _maxRelativeDepth;
        private StringBuilder _builder;
        
        public TreeJsonBuilder(Func<TNode, Dictionary<string, string>> toProperties, string childrenPropertyName = "Children")
        {
            _toProperties = toProperties;
            _childrenPropertyName = childrenPropertyName;
        }

        public TreeJsonBuilder(string childrenPropertyName = "Children") 
            : this(n => new Dictionary<string, string> { {"HierarchyId", n.HierarchyId.ToString("/").WrapDoubleQuotes()}}, childrenPropertyName)
        { }

        /// <summary>
        /// Build JSON representation of tree from given node
        /// </summary>
        /// <param name="node">Relative root node</param>
        /// <param name="includeRoot">Include relative root in JSON</param>
        /// <returns></returns>
        public string ToJson(TNode node, bool includeRoot = true)
        {
            return ToJson(node, n => true, int.MaxValue, includeRoot);
        }

        /// <summary>
        /// Build JSON representation of tree from given node with a filtering predicate
        /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <param name="node">Relative root node</param>
        /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
        /// <param name="includeRoot">Include relative root in JSON</param>
        /// <returns></returns>
        public string ToJson(TNode node, Func<TNode, bool> allowNext, bool includeRoot = true)
        {
            return ToJson(node, allowNext, int.MaxValue, includeRoot);
        }

        /// <summary>
        /// Build JSON representation of tree from given node to maximum relative depth
        /// </summary>
        /// <param name="node">Relative root node</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
        /// <param name="includeRoot">Include relative root in JSON</param>
        /// <returns></returns>
        public string ToJson(TNode node, int maxRelativeDepth, bool includeRoot = true)
        {
            return ToJson(node, n => true, maxRelativeDepth, includeRoot);
        }

        /// <summary>
        /// Build JSON representation of tree from given node with a filtering predicate to maximum relative depth.
        /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <param name="root">Relative root node</param>
        /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
        /// <param name="includeRoot">Include relative root in JSON</param>
        /// <returns></returns>
        public string ToJson(TNode root, Func<TNode, bool> allowNext, int maxRelativeDepth, bool includeRoot = true)
        {
            if (!allowNext(root) || maxRelativeDepth < 0) return string.Empty;

            _allowNext = allowNext;
            _maxRelativeDepth = maxRelativeDepth;
            _builder = new StringBuilder();

            if (includeRoot)
            {
                BuildItem(root, 0);
            }
            else
            {
                BuildChildren(root, 0, string.Empty);
            }

            return _builder.ToString();
        }

        private void BuildItem(TNode node, int curDepth)
        {
            _builder.Append("{");

            var propertyMap = _toProperties(node);

            var hasProperties = propertyMap.Count > 0;

            if (hasProperties)
            {
                _builder.Append(propertyMap.Select(kvp => $"{kvp.Key.WrapDoubleQuotes()}:{kvp.Value}").ToCsv());
            }

            BuildChildren(node, curDepth, $"{(hasProperties ? "," : string.Empty)}{_childrenPropertyName.WrapDoubleQuotes()}:");

            _builder.Append("}");
        }

        private void BuildChildren(TNode node, int curDepth, string prefix)
        {
            if (curDepth++ == _maxRelativeDepth) return;

            var effectiveChildren = node.Children.Where(_allowNext).ToArray();

            if (effectiveChildren.Length == 0) return;

            _builder.Append(prefix);
            _builder.Append("[");

            foreach (var child in effectiveChildren)
            {
                BuildItem(child, curDepth);

                if (child.NextSibling != null)
                {
                    _builder.Append(",");
                }
            }

            _builder.Append("]");
        }
    }
}
