using System;
using System.Collections.Generic;
using System.Linq;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Models.Method;

namespace Data.Core.Models.Class
{
    public class ClassVersionedMapping
        : AbstractVersionedMapping<ClassMapping, ClassVersionedMapping, ClassTypedMapping, ClassCommittedMappingEntry, ClassProposalMappingEntry, ClassReleaseMember>
    {
        public string Package { get; set; }

        public virtual ClassVersionedMapping Outer { get; set; }

        public virtual List<ClassVersionedMapping> InheritsFrom { get; set; }

        public virtual List<MethodVersionedMapping> Methods { get; set; }

        public virtual List<FieldVersionedMapping> Fields { get; set; }
    }
}
