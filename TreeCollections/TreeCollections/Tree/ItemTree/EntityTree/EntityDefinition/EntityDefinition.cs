
using System;
using System.Collections.Generic;

namespace TreeCollections
{
    public class EntityDefinition<TId, TItem> : IEntityDefinition<TId, TItem>
    {
        private readonly Func<TItem, TId> _getId;
         
        public EntityDefinition(Func<TItem, TId> getId,
                                IEqualityComparer<TId> idComparer,
                                IEqualityComparer<TItem> aliasComparer = null)
        {
            _getId = getId;
            IdEqualityComparer = idComparer;
            AliasEqualityComparer = aliasComparer ?? EqualityComparer<TItem>.Default;
        }

        public EntityDefinition(Func<TItem, TId> getId, IEqualityComparer<TItem> aliasComparer = null) 
            : this(getId, EqualityComparer<TId>.Default, aliasComparer) 
        { } 
        
        public TId GetId(TItem value) => _getId(value);
        
        public IEqualityComparer<TId> IdEqualityComparer { get; } 
        public IEqualityComparer<TItem> AliasEqualityComparer { get; } 
    }


    public class EntityDefinition<TId> : EntityDefinition<TId, TId>
    {
        public EntityDefinition(IEqualityComparer<TId> idEqualityComparer = null)
            : base(x => x, idEqualityComparer ?? EqualityComparer<TId>.Default, idEqualityComparer)
        { }
    }

    
    public class EntityDefinition<TId, TName, TItem> : EntityDefinition<TId, TItem>, IEntityDefinition<TId, TName, TItem>
    {
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
        
        public EntityDefinition(Func<TItem, TId> getId,
                                Func<TItem, TName> getName,
                                IEqualityComparer<TName> nameEqualityComparer = null)
            : this(getId, getName, null, nameEqualityComparer)
        { }

        public Func<TItem, TName> GetName { get; }
    }
    
    
    public class DualStateEntityDefinition<TId, TName, TItem> : EntityDefinition<TId, TItem>, IDualStateEntityDefinition<TId, TName, TItem>
    {
        public DualStateEntityDefinition(Func<TItem, TId> getId,
                                         Func<TItem, TName> getName,
                                         Func<TItem, bool> isEnabled,
                                         IEqualityComparer<TId> idComparer,
                                         IEqualityComparer<TName> nameComparer = null)
            : base(getId,
                   idComparer ?? EqualityComparer<TId>.Default,
                   new AliasDualStateComparer<TItem, TName>(getName, isEnabled, nameComparer ?? EqualityComparer<TName>.Default))
        {
            GetName = getName;
            IsEnabled = isEnabled;
        } 

        public DualStateEntityDefinition(Func<TItem, TId> getId,
                                         Func<TItem, TName> getName,
                                         Func<TItem, bool> isEnabled,
                                         IEqualityComparer<TName> nameComparer = null) 
            : this(getId, getName, isEnabled, null, nameComparer)
        { }

        public Func<TItem, TName> GetName { get; }
        public Func<TItem, bool> IsEnabled { get; } 
    }
}
