using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.EFCore.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="EntityEntry"/> class.
    /// </summary>
    public static class EntityEntryExtensions
    {

        /// <summary>
        /// Creates a human readable string representation of the key value of a given entity entry.
        /// </summary>
        /// <param name="entityEntry">The entity entry to create a human readable string representation of the key for.</param>
        /// <returns>The human readable string representation of the key value of the given entity entry.</returns>
        public static string GetKeyPropertiesAsString(EntityEntry entityEntry)
        {

        }
    }
}
