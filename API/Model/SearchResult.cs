using System;
using Data.Core.Models.Mapping;

namespace API.Model
{
    /// <summary>
    /// Model for a search result.
    /// </summary>
    public class SearchResult
    {
        public Guid MappingId { get; set; }

        public ComponentType Type { get; set; }

        public bool IsProposal { get; set; }

        public string Output { get; set; }

        public string Input { get; set; }

        public string MappingTypeName { get; set; }
    }
}
