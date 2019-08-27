using System;
using API.Model.Read.Core;

namespace API.Model.Read.Member
{
    public class MethodVersionedReadModel
        : AbstractVersionedReadModel
    {
        public Guid MemberOf { get; set; }
    }
}
