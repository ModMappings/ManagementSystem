using System;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a single mapping entry.
    /// Maps the input to the output from the input to the output type.
    /// </summary>
    public interface IMappingEntry
    {
        /// <summary>
        /// The id of the mapping entry.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The game version that this mapping is made for.
        /// </summary>
        IGameVersion GameVersion { get; set; }

        /// <summary>
        /// The input mappings type.
        /// </summary>
        string InputMappingType { get; set; }

        /// <summary>
        /// The actual input of the mapping.
        /// </summary>
        string InputMapping { get; set; }

        /// <summary>
        /// The output mappings type.
        /// </summary>
        string OutputMappingType { get; set; }

        /// <summary>
        /// The output of the mapping.
        /// </summary>
        string OutputMapping { get; set; }
    }
}
