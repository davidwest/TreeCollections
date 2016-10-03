
using System;
using System.Collections.Generic;
using System.Text;


namespace TreeCollections
{
    public static class TreeStringSerializationExtensions
    {
        public static string ToString<TNode>(this IEnumerable<TNode> seq, 
                                             Func<TNode, string> toTextLine, 
                                             int indention = 5) 
            where TNode : TreeNode<TNode>
        {
            var builder = new StringBuilder();
            
            seq.ForEach(n =>
            {
                var renderedIndent = string.Empty.PadRight(n.Level * indention);
                builder.AppendLine($"{renderedIndent}{toTextLine(n)}");
            });

            return builder.ToString();
        }

        
        public static string ToString<TNode>(this IEnumerable<TNode> seq, 
                                             Action<TNode, string, StringBuilder> appendLine, 
                                             int indention = 5) 
            where TNode : TreeNode<TNode>
        {
            var builder = new StringBuilder();

            seq.ForEach(n =>
            {
                var renderedIndent = string.Empty.PadRight(n.Level * indention);
                appendLine(n, renderedIndent, builder);
            });

            return builder.ToString();
        }
    }
}
