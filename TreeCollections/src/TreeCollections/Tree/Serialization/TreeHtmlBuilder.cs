
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeCollections
{
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

        public string ToHtml(TNode node, bool includeRoot = true)
        {
            return ToHtml(node, n => true, int.MaxValue, includeRoot);
        }

        public string ToHtml(TNode node, Func<TNode, bool> allowNext, bool includeRoot = true)
        {
            return ToHtml(node, allowNext, int.MaxValue, includeRoot);
        }
        
        public string ToHtml(TNode node, int maxRelativeDepth, bool includeRoot = true)
        {
            return ToHtml(node, n => true, maxRelativeDepth, includeRoot);
        }

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
