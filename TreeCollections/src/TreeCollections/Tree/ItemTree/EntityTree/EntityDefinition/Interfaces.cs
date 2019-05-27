using System;
using System.Collections.Generic;

namespace TreeCollections
{
    public interface IEntityDefinition<TId, in TItem>
    {
        TId GetId(TItem value);
        IEqualityComparer<TId> IdEqualityComparer { get; }
        IEqualityComparer<TItem> AliasEqualityComparer { get; } 
    }

    public interface IEntityDefinition<TId, out TName, in TItem> : IEntityDefinition<TId, TItem>
    {
        Func<TItem, TName> GetName { get; } 
    }
    
    public interface IDualStateEntityDefinition<TId, out TName, in TItem> : IEntityDefinition<TId, TName, TItem>
    {
        Func<TItem, bool> IsEnabled { get; } 
    }
}
