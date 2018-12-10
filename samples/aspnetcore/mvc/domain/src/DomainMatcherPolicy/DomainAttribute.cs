using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainMatcherPolicy
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class DomainAttribute : Attribute
    {
        public DomainAttribute(params string[] hosts)
        {
            Hosts = hosts?.ToArray() ?? Array.Empty<string>();
        }

        public IReadOnlyList<string> Hosts { get; }
    }
}
