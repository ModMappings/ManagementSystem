namespace Mcms.IO.Data
{
    /// <summary>
    /// Represents a field who's mapping is not yet part of the Mcms.
    /// Contains all data needed to import and export a given field.
    /// </summary>
    public class ExternalField
        : ExternalMapping
    {
        /// <summary>
        /// Indicates if this field is static.
        /// </summary>
        public bool IsStatic { get; set; } = false;

        /// <summary>
        /// The type of the field.
        /// </summary>
        public string Type { get; set; } = "";
    }
}
