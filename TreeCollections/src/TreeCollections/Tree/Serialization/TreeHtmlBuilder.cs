using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeCollections
{
    /// <summary>
    /// Builder for representing a tree as HTML
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public class TreeHtmlBuilder<TNode> where TNode : TreeNode<TNode>
    {
        private readonly HtmlBuildDefinition<TNode> _def;

        private Func<TNode, bool> _allowNext;
        private int _maxRelativeDepth;
        private StringBuilder _builder;

        public TreeHtmlBuilder(HtmlBuildDefinition<TNode> def = null)
        {
            _def = def ?? new HtmlBuildDefinition<TNode>();
        }

        /// <summary>
        /// Build HTML representation of tree from given node
        /// </summary>
        /// <param name="node">Relative root node</param>
        /// <param name="includeRoot">Include relative root in HTML</param>
        /// <returns></returns>
        public string ToHtml(TNode node, bool includeRoot = true)
        {
            return ToHtml(node, n => true, int.MaxValue, includeRoot);
        }

        /// <summary>
        /// Build HTML representation of tree from given node with a filtering predicate
        /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <param name="node">Relative root node</param>
        /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
        /// <param name="includeRoot">Include relative root in HTML</param>
        /// <returns></returns>
        public string ToHtml(TNode node, Func<TNode, bool> allowNext, bool includeRoot = true)
        {
            return ToHtml(node, allowNext, int.MaxValue, includeRoot);
        }

        /// <summary>
        /// Build HTML representation of tree from given node to maximum relative depth
        /// </summary>
        /// <param name="node">Relative root node</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
        /// <param name="includeRoot">Include relative root in HTML</param>
        /// <returns></returns>
        public string ToHtml(TNode node, int maxRelativeDepth, bool includeRoot = true)
        {
            return ToHtml(node, n => true, maxRelativeDepth, includeRoot);
        }

        /// <summary>
        /// Build HTML representation of tree from given node with a filtering predicate to maximum relative depth.
        /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
        /// </summary>
        /// <param name="root">Relative root node</param>
        /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
        /// <param name="maxRelativeDepth">Max depth of traversal (relative to root)</param>
        /// <param name="includeRoot">Include relative root in HTML</param>
        /// <returns></returns>
        public string ToHtml(TNode root, Func<TNode, bool> allowNext, int maxRelativeDepth, bool includeRoot = true)
        {
            if (!allowNext(root) || maxRelativeDepth < 0) return string.Empty;

            _allowNext = allowNext;
            _maxRelativeDepth = maxRelativeDepth;
            _builder = new StringBuilder();

            if (includeRoot)
            {
                _builder.Append($"<{_def.RootElementName}{SerializeAttributes(_def.GetRootAttributes(root))}>");
                _builder.Append(_def.GetRootPreHtml(root));

                BuildItem(root, 0);

                _builder.Append(_def.GetRootPostHtml(root));
                _builder.Append($"</{_def.RootElementName}>");
            }
            else
            {
                BuildChildren(root, 0);
            }
            
            return _builder.ToString();
        }

        private void BuildItem(TNode node, int curDepth)
        {
            _builder.Append($"<{_def.ItemElementName}{SerializeAttributes(_def.GetItemAttributes(node))}>");
            _builder.Append(_def.GetItemPreHtml(node));

            if (node.Children.Count != 0)
            {
                BuildChildren(node, curDepth);
            }
            
            _builder.Append(_def.GetItemPostHtml(node));
            _builder.Append($"</{_def.ItemElementName}>");
        }

        private void BuildChildren(TNode node, int curDepth)
        {
            if (curDepth++ == _maxRelativeDepth) return;

            _builder.Append($"<{_def.ContainerElementName}{SerializeAttributes(_def.GetContainerAttributes(node))}>");
            _builder.Append(_def.GetContainerPreHtml(node));

            foreach (var child in node.Children.Where(_allowNext))
            {
                BuildItem(child, curDepth);
            }

            _builder.Append(_def.GetContainerPostHtml(node));
            _builder.Append($"</{_def.ContainerElementName}>");
        }

        private static string SerializeAttributes(IDictionary<string, string> attributes)
        {
            return attributes.Count != 0 
                ? " " + attributes.Select(kvp => $"{kvp.Key}={kvp.Value.WrapDoubleQuotes()}").SerializeToString(" ") 
                : string.Empty;
        }
    }
}
