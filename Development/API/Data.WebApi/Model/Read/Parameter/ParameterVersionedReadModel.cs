using System;
using Data.WebApi.Model.Read.Core;

namespace Data.WebApi.Model.Read.Parameter
{
    public class ParameterVersionedReadModel
        : AbstractVersionedReadModel
    {
        public Guid ParameterOf { get; set; }

        public int Index { get; set; }
    }
}
