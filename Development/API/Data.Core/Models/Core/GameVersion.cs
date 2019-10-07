using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a single version of the game.
    /// </summary>
    public class GameVersion
    {
        /// <summary>
        /// The id of the version.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the version.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The moment the version was created within the system.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The id of the user who created the version.
        /// </summary>
        [Required]
        public virtual Guid CreatedBy { get; set; }

        /// <summary>
        /// Indicates if this version is a pre release or not.
        /// </summary>
        public bool IsPreRelease { get; set; }

        /// <summary>
        /// Indicates if this version is a snapshot or not.
        /// </summary>
        public bool IsSnapshot { get; set; }
    }
}
