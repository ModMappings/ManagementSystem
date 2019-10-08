namespace Data.Core.Models.Mapping.Component
{
    /// <summary>
    /// The type of mappable components.
    /// </summary>
    public enum ComponentType
    {
        /// <summary>
        /// Represents a package.
        /// </summary>
        PACKAGE,

        /// <summary>
        /// Represents a class.
        /// </summary>
        CLASS,

        /// <summary>
        /// Represents a method.
        /// </summary>
        METHOD,

        /// <summary>
        /// Represents a field.
        /// </summary>
        FIELD,

        /// <summary>
        /// Represents a parameter.
        /// </summary>
        PARAMETER
    }
}
