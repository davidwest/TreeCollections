using System;
using System.Collections.Generic;

namespace TreeCollections
{
    internal class AliasComparer<TItem, TName> : IEqualityComparer<TItem>
    {
        private readonly Func<TItem, TName> _getName;
        private readonly IEqualityComparer<TName> _nameComparer;
           
        public AliasComparer(Func<TItem, TName> getName, IEqualityComparer<TName> nameComparer)
        {
            _getName = getName;
            _nameComparer = nameComparer;
        } 

        public bool Equals(TItem x, TItem y)
        {
            return _nameComparer.Equals(_getName(x), _getName(y));
        }

        public int GetHashCode(TItem obj)
        {
            return _nameComparer.GetHashCode(_getName(obj));
        }
    }
}
