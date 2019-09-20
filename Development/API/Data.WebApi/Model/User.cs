using System;

namespace Data.WebApi.Model
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool CanEdit { get; set; }

        public bool CanReview { get; set; }

        public bool CanCommit { get; set; }

        public bool CanRelease { get; set; }

        public bool CanCreateGameVersions { get; set; }

        public bool CanCreateMappingTypes { get; set; }
    }
}
