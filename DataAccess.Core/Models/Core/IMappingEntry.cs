namespace DataAccess.Core.Models.Core
{
    public interface IMappingEntry
    {
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
