using System;
using API.Model.View.Core;

namespace API.Model.View.Parameter
{
    public class ParameterVersionedViewModel
        : AbstractVersionedViewModel
    {
        public Guid ParameterOf { get; set; }

        public int Index { get; set; }
    }
}
