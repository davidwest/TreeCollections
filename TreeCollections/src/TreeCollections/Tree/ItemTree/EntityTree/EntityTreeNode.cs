using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    /// <summary>
    /// Abstract tree node that refines ItemTreeNode by allowing the enforcement of uniqueness constraints
    /// based on properties of its payload item (entity).
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public abstract partial class EntityTreeNode<TNode, TId, TItem> : ItemTreeNode<TNode, TItem>, IEntityTreeNode<TId, TItem> 
        where TNode: EntityTreeNode<TNode, TId, TItem>
    {
        /// <summary>
        /// Root constructor
        /// </summary>
        /// <param name="definition">Entity definition that determines identity parameters</param>
        /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
        /// <param name="rootItem">Item contained by root</param>
        protected EntityTreeNode(IEntityDefinition<TId, TItem> definition,
                                 ErrorCheckOptions checkOptions,
                                 TItem rootItem)
            : base(rootItem, null)
        {
            Definition = definition;
            CheckOptions = checkOptions;

            TreeIdMap = 
                checkOptions.HasFlag(ErrorCheckOptions.TreeScopeIdDuplicates) 
                    ? new HashSet<TId>(definition.IdEqualityComparer) 
                    : null;
        }
        
        /// <summary>
        /// Descendant constructor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        protected EntityTreeNode(TItem item, TNode parent)
            : base(item, parent)
        {
            Definition = parent.Definition;
            CheckOptions = parent.CheckOptions;

            TreeIdMap = parent.TreeIdMap;
        }

        public IdentityError Error { get; internal set; }

        public TNode this[TId id] => this.FirstOrDefault(n => n.HasEquivalentId(id));
        
        public TId Id => Definition.GetId(Item);

        
        // ----- Shared by all nodes in tree -----
        internal HashSet<TId> TreeIdMap { get; set; }
        // --------------------------------------------------


        protected virtual bool ContinueAddOnExistingError() => false;

        protected IEntityDefinition<TId, TItem> Definition { get; }

        protected ErrorCheckOptions CheckOptions { get; }

        protected sealed override bool OnAddCanProceed()
        {
            return Error == IdentityError.None || ContinueAddOnExistingError();
        }
        
        protected sealed override void SetChildErrorsOnAttachment()
        {
            if (CheckOptions.HasFlag(ErrorCheckOptions.CyclicIdDuplicates))
            {
                SetChildCyclicIdErrorsOnAttachment();
            }

            if (CheckOptions.HasFlag(ErrorCheckOptions.SiblingAliasDuplicates))
            {
                SetChildSiblingAliasErrorsOnAttachment();
            }

            ILookup<TId, TNode> nodesGroupedById = null;

            if (CheckOptions.HasFlag(ErrorCheckOptions.SiblingIdDuplicates))
            {
                nodesGroupedById = Children.ToLookup(c => c.Id, Definition.IdEqualityComparer);

                var siblingDuplicateIdNodes =
                    nodesGroupedById
                    .Where(grp => grp.Count() > 1)
                    .SelectMany(n => n);

                siblingDuplicateIdNodes.ForEach(n => n.Error |= IdentityError.SiblingIdDuplicate);
            }
            
            if (!IdentityTrackingIsTreeScope) return;
            
            nodesGroupedById = nodesGroupedById ?? Children.ToLookup(c => c.Id, Definition.IdEqualityComparer);

            var duplicateNodeIds =
                from grp in nodesGroupedById
                let hasBeenRegistered = TreeIdMap.Contains(grp.Key)
                where hasBeenRegistered
                select grp.Key;

            var treeScopeIdDuplicateNodes =
                from node in Root
                where duplicateNodeIds.Contains(node.Id)
                select node;

            treeScopeIdDuplicateNodes.ForEach(n => n.Error |= IdentityError.TreeScopeIdDuplicate);

            nodesGroupedById.ForEach(grp => TreeIdMap.Add(grp.Key));
        }
        
        private protected bool IdentityTrackingIsTreeScope => TreeIdMap != null;

        private protected bool HasEquivalentId(TId otherId) => Definition.IdEqualityComparer.Equals(Id, otherId);

        private void SetChildCyclicIdErrorsOnAttachment()
        {
            var cyclicErrorPairs =
                from pn in SelectPathUpward()
                from cn in Children
                where pn.HasEquivalentId(cn.Id)
                select new { PathNode = pn, ChildNode = cn };

            foreach (var pair in cyclicErrorPairs)
            {
                pair.PathNode.Error |= IdentityError.CyclicIdDuplicate;
                pair.ChildNode.Error |= IdentityError.CyclicIdDuplicate;
            }
        }

        private void SetChildSiblingAliasErrorsOnAttachment()
        {
            var duplicateAliasNodes =
                Children
                .ToLookup(c => c.Item, Definition.AliasEqualityComparer)
                .Where(grp => grp.Count() > 1)
                .SelectMany(n => n);

            duplicateAliasNodes.ForEach(n => n.Error |= IdentityError.SiblingAliasDuplicate);
        }
    }
}
