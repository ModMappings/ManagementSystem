using System;
using Data.WebApi.Model.Read.Core;

namespace Data.WebApi.Model.Read.Field
{
    public class FieldVersionedReadModel
        : AbstractVersionedReadModel
    {
        public Guid MemberOf { get; set; }

        public bool IsStatic { get; set; }
    }
}
