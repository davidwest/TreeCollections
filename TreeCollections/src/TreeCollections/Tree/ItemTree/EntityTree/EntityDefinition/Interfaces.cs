using System;
using System.Collections.Generic;

namespace TreeCollections
{
    /// <summary>
    /// Meta info that describes an entity
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public interface IEntityDefinition<TId, in TItem>
    {
        TId GetId(TItem value);
        IEqualityComparer<TId> IdEqualityComparer { get; }
        IEqualityComparer<TItem> AliasEqualityComparer { get; } 
    }

    /// <summary>
    /// Meta info that describes an entity with an explicit alias (name, title, etc.)
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public interface IEntityDefinition<TId, out TName, in TItem> : IEntityDefinition<TId, TItem>
    {
        Func<TItem, TName> GetName { get; } 
    }
}
