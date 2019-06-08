using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    public static class TreeQueryExtensions
    {
        /// <summary>
        /// Filters nodes based on a predicate, including distinct ancestors (matching the predicate or not)
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <param name="seq"></param>
        /// <param name="satisfiesCondition"></param>
        /// <returns></returns>
        public static IEnumerable<TNode> WhereSupports<TNode>(this IEnumerable<TNode> seq, Func<TNode, bool> satisfiesCondition) 
            where TNode : TreeNode<TNode>
        {
            return seq
                   .Where(satisfiesCondition)
                   .SelectMany(n => n.SelectPathUpward())
                   .Distinct();
        } 
    }
}
