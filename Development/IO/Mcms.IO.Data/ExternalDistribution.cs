namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents the distribution that the mapping is part of.
    /// Not all mapping types, contain this information as such UNKNOWN is an option.
    /// </summary>
    public enum ExternalDistribution
    {
        /// <summary>
        /// The component that is being mapped is part of both distributions.
        /// </summary>
        BOTH,

        /// <summary>
        /// The component that is being mapped is part of the server distribution only.
        /// </summary>
        SERVER_ONLY,

        /// <summary>
        /// The component that is being mapped is part of the client distribution only.
        /// </summary>
        CLIENT_ONLY,

        /// <summary>
        /// It is unknown if the component that is being mapped is part of the client, server or both distributions.
        /// </summary>
        UNKNOWN
    }
}
