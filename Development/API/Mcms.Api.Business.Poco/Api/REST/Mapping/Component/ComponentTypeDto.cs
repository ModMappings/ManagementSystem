namespace Mcms.Api.Business.Poco.Api.REST.Mapping.Component
{
    /// <summary>
    /// Represents a single type of a component,
    /// as such it represents a type of a piece of source code that can be remapped.
    /// </summary>
    public enum ComponentTypeDto
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
