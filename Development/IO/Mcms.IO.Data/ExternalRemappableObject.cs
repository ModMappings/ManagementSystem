namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents a remappable object that is not part of the Mcms.
    /// </summary>
    public abstract class ExternalMapping
    {

        /// <summary>
        /// The input of the mapping.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// The output of the mapping
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// The documentation that accompanies the mapping.
        /// </summary>
        public string Documentation { get; set; } = "";

        /// <summary>
        /// The distribution that this mapping is part of.
        /// Not all mappings have a distribution specified.
        /// </summary>
        public ExternalDistribution Distribution { get; set; } = ExternalDistribution.UNKNOWN;
    }
}
