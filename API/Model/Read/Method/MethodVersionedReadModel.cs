using System;
using API.Model.Read.Core;

namespace API.Model.Read.Method
{
    /// <summary>
    /// Versioned read model for methods.1
    /// </summary>
    public class MethodVersionedReadModel
        : AbstractVersionedReadModel
    {
        /// <summary>
        /// The id of the versioned class model that this method is part of.
        /// </summary>
        public Guid MemberOf { get; set; }

        /// <summary>
        /// The descriptor of the method.
        /// </summary>
        public string Descriptor { get; set; }

        /// <summary>
        /// Indicates if this method is a static member.
        /// </summary>
        public bool IsStatic { get; set; }
    }
}
