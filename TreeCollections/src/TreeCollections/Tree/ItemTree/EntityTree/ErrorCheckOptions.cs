using System;

namespace TreeCollections
{
    [Flags]
    public enum ErrorCheckOptions
    {
        None = 0,
        CyclicIdDuplicates = 0x1,
        SiblingIdDuplicates = 0x2,
        SiblingAliasDuplicates = 0x4,
        TreeScopeIdDuplicates = 0x8,

        Default = 0x1 | 0x2 | 0x4,
        All = 0x1 | 0x2 | 0x4 | 0x8
    }
}
