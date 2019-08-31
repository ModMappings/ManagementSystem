using System;
using API.Model.Read.Core;

namespace API.Model.Read.Field
{
    public class FieldVersionedReadModel
        : AbstractVersionedReadModel
    {
        public Guid MemberOf { get; set; }

        public bool IsStatic { get; set; }
    }
}
