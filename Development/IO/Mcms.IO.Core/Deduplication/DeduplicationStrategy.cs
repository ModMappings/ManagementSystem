namespace Mcms.IO.Core.Deduplication
{
    /// <summary>
    /// The strategies that are available inside the Mcms IO system to help with
    /// deduplication.
    /// </summary>
    public enum DeduplicationStrategy
    {
        /// <summary>
        /// Indicates a strategy that forces the creation of a new object always.
        /// </summary>
        ALWAYS_CREATE,
        
        /// <summary>
        /// Indicates a strategy that forces the system to scan for a duplicate using the input mapping.
        /// If one exists merge the data logically, or if none exists create a new object.
        /// </summary>
        INPUT_UNIQUE,
        
        /// <summary>
        /// Indicates a strategy that forces the system to scan for a duplicate using the output mapping.
        /// If one exists merge the data logically, or if none exists create a new object.
        /// </summary>
        OUTPUT_UNIQUE,
        
        /// <summary>
        /// Indicates a strategy that forces the system to scan for a duplicate using the input mapping first,
        /// if none is found then it will attempt to search for a duplicate using the output mapping.
        /// If a duplicate is found it the data is merged logically, else a new object is created. 
        /// </summary>
        BOTH_UNIQUE
    }
}
