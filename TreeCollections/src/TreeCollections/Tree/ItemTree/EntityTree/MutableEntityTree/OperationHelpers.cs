using System;

namespace TreeCollections
{
    public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    {
        private void MoveToNonSiblingAdjacentPosition(TNode targetNode, Adjacency adjacency)
        {
            OnNodeReparenting(targetNode);

            var targetIndex = targetNode.OrderIndex + (adjacency == Adjacency.Before ? 0 : 1);
            
            var newParent = targetNode.Parent;

            Detach();
            newParent.AttachChildOnMove(This, targetIndex);
        }


        private void MoveToSiblingAdjacentPosition(TNode targetNode, Adjacency adjacency)
        {
            var curIndex = OrderIndex;
            var targetIndex = targetNode.OrderIndex;

            var diff = targetIndex - curIndex;
            if (diff == 0) return;

            targetIndex += (diff < 0 ? 0 : -1) + (adjacency == Adjacency.Before ? 0 : 1);

            Parent._children.Remove(This);
            Parent._children.Insert(targetIndex, This);

            Parent.SetChildrenSiblingReferences();

            Parent.OnChildrenReordered();
        }

        
        private void AttachChildOnAdd(TNode node, int? insertIndex = null)
        {
            InnerAddChild(node, insertIndex);

            SetChildrenSiblingReferences();

            node.SetErrorsAfterAddingThis();
            node.OnNodeAttached();
        }

        
        private void AttachChildOnMove(TNode node, int? insertIndex = null)
        {
            node.Parent = This;

            InnerAddChild(node, insertIndex);

            SetChildrenSiblingReferences();

            foreach (var n in node)
            {
                n.Level = n.Parent.Level + 1;
                n.Root = Root;
                n.TreeIdMap = TreeIdMap;
            }
            
            node.SetErrorsAfterMovingThis();
            node.OnNodeAttached();
            node.OnNodeReparented();
        }

        
        private void InnerAddChild(TNode node, int? insertIndex)
        {
            if (insertIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(insertIndex));
            }

            if (!insertIndex.HasValue || insertIndex.Value >= _children.Count)
            {
                _children.Add(node);
            }
            else
            {
                _children.Insert(insertIndex.Value, node);
            }
        }


        private bool HasSameIdentityAs(TNode other) => HasEquivalentId(other.Id);
        private bool HasSameAliasAs(TNode other) => Definition.AliasEqualityComparer.Equals(Item, other.Item);

        private bool IsCompatible(TNode externalNode)
        {
            return externalNode.Definition.Equals(Definition) &&
                   externalNode.CheckOptions == CheckOptions;
        }
    }
}

