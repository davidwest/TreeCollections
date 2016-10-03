
using System;
using System.Collections.Generic;

namespace TreeCollections
{
    public class HtmlBuildDefinition<TNode> where TNode : TreeNode<TNode>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly IDictionary<string, string> EmptyAttributes = new Dictionary<string, string>();

        public HtmlBuildDefinition()
        {
            RootElementName = "ul";
            GetRootAttributes = n => EmptyAttributes;
            GetRootPreHtml = n => string.Empty;
            GetRootPostHtml = n => string.Empty;

            ItemElementName = "li";
            GetItemAttributes = n => EmptyAttributes;
            GetItemPreHtml = n => n.HierarchyId.ToString("/");
            GetItemPostHtml = n => string.Empty;
            
            ContainerElementName = "ul";
            GetContainerAttributes = n => EmptyAttributes;
            GetContainerPreHtml = n => string.Empty;
            GetContainerPostHtml = n => string.Empty;
        }

        public string RootElementName { get; set; }
        public Func<TNode, IDictionary<string, string>> GetRootAttributes { get; set; }
        public Func<TNode, string> GetRootPreHtml { get; set; }
        public Func<TNode, string> GetRootPostHtml { get; set; }

        public string ItemElementName { get; set; }
        public Func<TNode, IDictionary<string, string>> GetItemAttributes { get; set; }
        public Func<TNode, string> GetItemPreHtml { get; set; }
        public Func<TNode, string> GetItemPostHtml { get; set; }

        public string ContainerElementName { get; set; }
        public Func<TNode, IDictionary<string, string>> GetContainerAttributes { get; set; }
        public Func<TNode, string> GetContainerPreHtml { get; set; }
        public Func<TNode, string> GetContainerPostHtml { get; set; }
    } 
}
