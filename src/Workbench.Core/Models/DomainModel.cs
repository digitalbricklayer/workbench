using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class DomainModel : Model
    {
        /// <summary>
        /// Initialize a domain entity with a name.
        /// </summary>
        /// <param name="theName">Domain name.</param>
        protected DomainModel(ModelName theName)
            : base(theName)
        {
            Contract.Requires<ArgumentNullException>(theName != null);
        }

        /// <summary>
        /// Initialize a domain entity with default values.
        /// </summary>
        protected DomainModel()
        {
        }
    }
}
