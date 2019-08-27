using System;
using API.Model.View.Core;

namespace API.Model.View.Member
{
    public class MethodVersionedViewModel
        : AbstractVersionedViewModel
    {
        public Guid MemberOf { get; set; }
    }
}
