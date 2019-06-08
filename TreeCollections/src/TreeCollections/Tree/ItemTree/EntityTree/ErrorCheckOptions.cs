using System;

namespace TreeCollections
{
    [Flags]
    public enum ErrorCheckOptions
    {
        /// <summary>
        /// No error checks
        /// </summary>
        None = 0,

        /// <summary>
        /// Check for Id cycles (duplicate Ids in single paths)
        /// </summary>
        CyclicIdDuplicates = 0x1,

        /// <summary>
        /// Check for Id duplicates among siblings
        /// </summary>
        SiblingIdDuplicates = 0x2,

        /// <summary>
        /// Check for alias (name) duplicates among siblings
        /// </summary>
        SiblingAliasDuplicates = 0x4,

        /// <summary>
        /// Check for Id duplicates across entire tree.
        /// If this flag is set, CyclicIdDuplicates and SiblingIdDuplicates are ignored because both of these checks will be covered.
        /// </summary>
        TreeScopeIdDuplicates = 0x8,

        Default = 0x4 | 0x8,
        All = 0x1 | 0x2 | 0x4 | 0x8
    }
}
