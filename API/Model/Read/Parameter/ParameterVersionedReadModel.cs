using System;
using API.Model.Read.Core;

namespace API.Model.Read.Parameter
{
    public class ParameterVersionedReadModel
        : AbstractVersionedReadModel
    {
        public Guid ParameterOf { get; set; }

        public int Index { get; set; }
    }
}
