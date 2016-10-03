
using System.Collections.Generic;
using System.Linq;

namespace TreeCollections
{
    // ReSharper disable once UnusedTypeParameter
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        private void SetErrorsAfterAddingThis()
        {
            SetSiblingErrors();

            if (CheckOptions.HasFlag(ErrorCheckOptions.CyclicIdDuplicates))
            {
                SetCyclicIdErrors();
            }
            
            if (IdentityTrackingIsTreeScope)
            {
                SetTreeScopeIdErrors();
            }
        }


        private void SetErrorsAfterMovingThis()
        {
            SetSiblingErrors();

            if (CheckOptions.HasFlag(ErrorCheckOptions.CyclicIdDuplicates))
            {
                SetCyclicIdErrorsForEach();
            }

            if (IdentityTrackingIsTreeScope)
            {
                SetTreeScopeIdErrorsForEach();
            }
        }

        
        private void SetSiblingErrors()
        {
            TNode[] siblings = null;

            if (CheckOptions.HasFlag(ErrorCheckOptions.SiblingIdDuplicates))
            {
                siblings = SelectSiblings().ToArray();

                var existingIdMatch = siblings.FirstOrDefault(HasSameIdentityAs);

                if (existingIdMatch != null)
                {
                    Error |= IdentityError.SiblingIdDuplicate;
                    existingIdMatch.Error |= IdentityError.SiblingIdDuplicate;
                }
            }

            if (!CheckOptions.HasFlag(ErrorCheckOptions.SiblingAliasDuplicates)) return;

            siblings = siblings ?? SelectSiblings().ToArray();

            var existingAliasMatch = siblings.FirstOrDefault(HasSameAliasAs);

            if (existingAliasMatch != null)
            {
                Error |= IdentityError.SiblingAliasDuplicate;
                existingAliasMatch.Error |= IdentityError.SiblingAliasDuplicate;
            }
        }


        private void SetCyclicIdErrorsForEach()
        {
            this.ForEach(n => n.SetCyclicIdErrors());
        }

        private void SetCyclicIdErrors()
        {
            var existingMatch = 
                SelectAncestorsUpward()
                .FirstOrDefault(HasSameIdentityAs);

            if (existingMatch == null) return;

            Error |= IdentityError.CyclicIdDuplicate;
            existingMatch.Error |= IdentityError.CyclicIdDuplicate;
        }


        private void SetTreeScopeIdErrorsForEach()
        {
            // TODO: could optimize

            this.ForEach(n => n.SetTreeScopeIdErrors());
        }

        private void SetTreeScopeIdErrors()
        {
            if (!TreeIdMap.Contains(Id))
            {
                TreeIdMap.Add(Id);
                return;
            }

            var duplicates = Root.Where(n => n.HasEquivalentId(Id));

            duplicates.ForEach(dup => dup.Error |= IdentityError.TreeScopeIdDuplicate);
        }


        private void UpdateErrorsBeforeDetachingThis()
        {
            if (CheckOptions.HasFlag(ErrorCheckOptions.CyclicIdDuplicates))
            {
                UpdateCyclicIdErrorsBeforeDetachingThis();
            }

            if (CheckOptions.HasFlag(ErrorCheckOptions.SiblingAliasDuplicates) &&
                Error.HasFlag(IdentityError.SiblingAliasDuplicate))
            {
                UpdateSiblingAliasErrorsBeforeDetachingThis();
            }

            if (CheckOptions.HasFlag(ErrorCheckOptions.SiblingIdDuplicates) &&
                Error.HasFlag(IdentityError.SiblingIdDuplicate))
            {
                UpdateSiblingIdErrorsBeforeDetachingThis();
            }

            if (IdentityTrackingIsTreeScope)
            {
                UpdateTreeScopeIdErrorsBeforeDetachingThis();
            }            
        }


        private void UpdateCyclicIdErrorsBeforeDetachingThis()
        {
            var nodesWithCycles = SelectDescendants().Where(n => n.Error.HasFlag(IdentityError.CyclicIdDuplicate));

            foreach (var node in nodesWithCycles)
            {
                var path = node.SelectPathUpward().ToArray();

                // TODO: could optimize

                var duplicatesAboveThis = path.Where(n => n.Level < Level && node.HasSameIdentityAs(n)).ToArray();
                var containedDuplicates = path.Where(n => n.Level >= Level && node.HasSameIdentityAs(n)).ToArray();
                
                if (duplicatesAboveThis.Length == 1)
                {
                    duplicatesAboveThis[0].Error &= ~IdentityError.CyclicIdDuplicate;
                }

                if (containedDuplicates.Length == 1)
                {
                    node.Error &= ~IdentityError.CyclicIdDuplicate;
                }
            }
        }


        private void UpdateSiblingIdErrorsBeforeDetachingThis()
        {
            Error &= ~IdentityError.SiblingIdDuplicate;

            var siblingDuplicates = SelectSiblings().Where(HasSameIdentityAs).ToArray();

            if (siblingDuplicates.Length == 1)
            {
                siblingDuplicates[0].Error &= ~IdentityError.SiblingIdDuplicate;
            }
        }


        private void UpdateSiblingAliasErrorsBeforeDetachingThis()
        {
            Error &= ~IdentityError.SiblingAliasDuplicate;

            var siblingDuplicates = SelectSiblings().Where(HasSameAliasAs).ToArray();

            if (siblingDuplicates.Length == 1)
            {
                siblingDuplicates[0].Error &= ~IdentityError.SiblingAliasDuplicate;
            }
        }


        private void UpdateTreeScopeIdErrorsBeforeDetachingThis()
        {
            var treeIdGroups = Root.ToLookup(n => n.Id);

            foreach (var grp in treeIdGroups)
            {
                var id = grp.Key;

                var enumerated = grp.ToArray();
                var insiders = enumerated.Where(n => n.Equals(This) || n.IsDescendantOf(This)).ToArray();

                if (insiders.Length == 0) continue;

                if (enumerated.Length == insiders.Length)
                {
                    TreeIdMap.Remove(id);
                    continue;
                }

                var outsiders = enumerated.Except(insiders).ToArray();

                if (outsiders.Length == 1)
                {
                    outsiders[0].Error &= ~IdentityError.TreeScopeIdDuplicate;
                }

                if (insiders.Length == 1)
                {
                    insiders[0].Error &= ~IdentityError.TreeScopeIdDuplicate;
                }
            }

            TreeIdMap = new HashSet<TId>(Definition.IdEqualityComparer);
            this.Select(n => n.Id).ForEach(id => TreeIdMap.Add(id));
        }
    }
}
