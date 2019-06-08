using System;
using System.Collections.Generic;

namespace TreeCollections
{
    /// <summary>
    /// Meta info that describes an entity with an explicit string name.
    /// Used by tree nodes to enforce entity constraints.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class NamedEntityDefinition<TId, TItem> : EntityDefinition<TId, string, TItem>
    {
        /// <summary>
        /// Constructor with explicit Id comparer.
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="getName">Entity name selector</param>
        /// <param name="idComparer">Entity Id comparer</param>
        /// <param name="nameComparer">Entity name comparer (acting as alias comparer)</param>
        public NamedEntityDefinition(Func<TItem, TId> getId, 
                                     Func<TItem, string> getName, 
                                     IEqualityComparer<TId> idComparer, 
                                     IEqualityComparer<string> nameComparer = null) 
            : base(getId, getName, idComparer, nameComparer)
        { }

        /// <summary>
        /// Constructor with default Id comparer.
        /// </summary>
        /// <param name="getId">Entity Id selector</param>
        /// <param name="getName">Entity name selector</param>
        /// <param name="nameComparer">Entity name comparer (acting as alias comparer)</param>
        public NamedEntityDefinition(Func<TItem, TId> getId, 
                                     Func<TItem, string> getName, 
                                     IEqualityComparer<string> nameComparer = null) 
            : base(getId, getName, nameComparer)
        { }
    }
}
