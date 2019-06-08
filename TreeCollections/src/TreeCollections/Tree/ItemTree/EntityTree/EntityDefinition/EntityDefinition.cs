using System;
using System.Collections.Generic;

namespace TreeCollections
{
    /// <summary>
    /// Meta info that describes an entity.
    /// Used by tree nodes to enforce entity constraints.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class EntityDefinition<TId, TItem> : IEntityDefinition<TId, TItem>
    {
        private readonly Func<TItem, TId> _getId;
         
        /// <summary>
        /// Constructor with explicit Id Comparer
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="idComparer">Entity Id comparer</param>
        /// <param name="aliasComparer">Entity alias comparer (without explicit name)</param>
        public EntityDefinition(Func<TItem, TId> getId,
                                IEqualityComparer<TId> idComparer,
                                IEqualityComparer<TItem> aliasComparer = null)
        {
            _getId = getId;
            IdEqualityComparer = idComparer;
            AliasEqualityComparer = aliasComparer ?? EqualityComparer<TItem>.Default;
        }

        /// <summary>
        /// Constructor with default Id Comparer
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="aliasComparer">Entity alias comparer (without explicit name)</param>
        public EntityDefinition(Func<TItem, TId> getId, IEqualityComparer<TItem> aliasComparer = null) 
            : this(getId, EqualityComparer<TId>.Default, aliasComparer) 
        { } 
        
        public TId GetId(TItem value) => _getId(value);
        
        public IEqualityComparer<TId> IdEqualityComparer { get; } 
        public IEqualityComparer<TItem> AliasEqualityComparer { get; } 
    }

    /// <summary>
    /// Meta info that describes an entity with an explicit name of arbitrary type.
    /// Used by tree nodes to enforce entity constraints.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class EntityDefinition<TId, TName, TItem> : EntityDefinition<TId, TItem>, IEntityDefinition<TId, TName, TItem>
    {
        /// <summary>
        /// Constructor with explicit Id comparer
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="getName">Entity name selector</param>
        /// <param name="idComparer">Entity id comparer</param>
        /// <param name="nameComparer">Name comparer (acting as alias comparer)</param>
        public EntityDefinition(Func<TItem, TId> getId,
                                Func<TItem, TName> getName,
                                IEqualityComparer<TId> idComparer,
                                IEqualityComparer<TName> nameComparer = null)
            : base(getId,
                   idComparer ?? EqualityComparer<TId>.Default,
                   nameComparer != null
                       ? new AliasComparer<TItem, TName>(getName, nameComparer)
                       : null)
        {
            GetName = getName;
        }

        /// <summary>
        /// Constructor with default Id comparer.
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="getName">Entity name selector</param>
        /// <param name="nameComparer">Name comparer (acting as alias comparer)</param>
        public EntityDefinition(Func<TItem, TId> getId,
                                Func<TItem, TName> getName,
                                IEqualityComparer<TName> nameComparer = null)
            : this(getId, getName, null, nameComparer)
        { }

        public Func<TItem, TName> GetName { get; }
    }
}
