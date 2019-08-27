using System;
using API.Model.View.Core;

namespace API.Model.View.Field
{
    public class FieldVersionedViewModel
        : AbstractVersionedViewModel
    {
        public Guid MemberOf { get; set; }
    }
}
