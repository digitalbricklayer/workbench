using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Named reference to a shared domain.
    /// </summary>
    [Serializable]
    public class SharedDomainReference
    {
        /// <summary>
        /// Initialize a shared domain reference with a shared domain name.
        /// </summary>
        /// <param name="sharedDomainName">Name of the shared domain.</param>
        public SharedDomainReference(string sharedDomainName)
        {
            if (string.IsNullOrWhiteSpace(sharedDomainName))
                throw new ArgumentException("sharedDomainName");
            this.DomainName = sharedDomainName;
        }

        public SharedDomainReference()
        {
            
        }

        /// <summary>
        /// Gets the shared domain name.
        /// </summary>
        public string DomainName { get; private set; }
    }
}
