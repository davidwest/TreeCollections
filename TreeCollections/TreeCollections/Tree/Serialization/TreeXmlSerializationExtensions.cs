
using System;
using System.Linq;
using System.Xml.Linq;

namespace TreeCollections
{
    public static class TreeXmlSerializationExtensions
    {        
        public static void CopyTo<TNode>(this TNode parent, 
                                         XContainer xParent, 
                                         Func<TNode, XElement> convert, 
                                         Func<TNode, bool> allowNext, 
                                         int? maxRelativeDepth = null)
            where TNode : TreeNode<TNode>
        {
            if (!allowNext(parent) || maxRelativeDepth <= 0) return;

            BuildXml(parent, xParent, convert, allowNext, 0, maxRelativeDepth ?? int.MaxValue);
        }


        public static void CopyTo<TNode>(this TNode parent,
                                         XContainer xParent,
                                         Func<TNode, XElement> convert,
                                         int? maxRelativeDepth = null)
            where TNode : TreeNode<TNode>
        {
            parent.CopyTo(xParent, convert, n => true, maxRelativeDepth);
        }


        private static void BuildXml<TNode>(TNode parent, 
                                            XContainer xParent, 
                                            Func<TNode, XElement> convert, 
                                            Func<TNode, bool> allowNext, 
                                            int curDepth, 
                                            int maxRelativeDepth) 
            where TNode : TreeNode<TNode>
        {
            if (curDepth++ == maxRelativeDepth) return;
            
            foreach (var child in parent.Children.Where(allowNext))
            {
                var xNode = convert(parent);
                xParent.Add(xNode);

                BuildXml(child, xNode, convert, allowNext, curDepth, maxRelativeDepth);
            }
        }
    }
}
