using System;

namespace Mcms.Api.Business.Core.Manager.Filter
{
    public class FilterConfigurationException : Exception
    {
        public FilterConfigurationException() : base("The configuration of the filter is not valid.")
        {
        }
    }
}
