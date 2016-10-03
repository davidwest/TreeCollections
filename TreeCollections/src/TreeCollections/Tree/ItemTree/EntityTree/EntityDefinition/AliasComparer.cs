
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
    
    
    internal class AliasDualStateComparer<TItem, TName> : IEqualityComparer<TItem>
    {
        private readonly Func<TItem, TName> _getName;
        private readonly Func<TItem, bool> _isEnabled; 
        private readonly IEqualityComparer<TName> _nameComparer;  

        public AliasDualStateComparer(Func<TItem, TName> getName, 
                                      Func<TItem, bool> isEnabled, 
                                      IEqualityComparer<TName> nameEqualityComparer)
        {
            _getName = getName;
            _isEnabled = isEnabled;
            _nameComparer = nameEqualityComparer;
        } 

        public bool Equals(TItem x, TItem y)
        {
            return _nameComparer.Equals(_getName(x), _getName(y)) && _isEnabled(x) == _isEnabled(y);
        }

        public int GetHashCode(TItem obj)
        {
            var hash = 17;
            hash *= 31 + _nameComparer.GetHashCode(_getName(obj));
            hash *= 31 + _isEnabled(obj).GetHashCode();

            return hash;
        }
    }
}
