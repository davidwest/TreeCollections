using System;
using System.Collections.Generic;

namespace TreeCollections
{
    /// <summary>
    /// Configuration that describes how to build HTML from a tree
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
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

        /// <summary>
        /// Name of HTML element to use for root. Default : "ul".
        /// </summary>
        public string RootElementName { get; set; }

        public Func<TNode, IDictionary<string, string>> GetRootAttributes { get; set; }

        public Func<TNode, string> GetRootPreHtml { get; set; }

        public Func<TNode, string> GetRootPostHtml { get; set; }


        /// <summary>
        /// Name of HTML element to use for individual item (node).  Default: "li".
        /// </summary>
        public string ItemElementName { get; set; }

        public Func<TNode, IDictionary<string, string>> GetItemAttributes { get; set; }

        public Func<TNode, string> GetItemPreHtml { get; set; }

        public Func<TNode, string> GetItemPostHtml { get; set; }


        /// <summary>
        /// Name of HTML element to use for nested item (node) container. Default: "ul".
        /// </summary>
        public string ContainerElementName { get; set; }

        public Func<TNode, IDictionary<string, string>> GetContainerAttributes { get; set; }

        public Func<TNode, string> GetContainerPreHtml { get; set; }

        public Func<TNode, string> GetContainerPostHtml { get; set; }
    } 
}
