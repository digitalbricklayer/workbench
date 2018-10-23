using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core
{
    public class ModelValidationContext
    {
        private readonly List<string> errors;

        /// <summary>
        /// Initialize a new validation context with default values.
        /// </summary>
        public ModelValidationContext()
        {
            this.errors = new List<string>();
        }

        /// <summary>
        /// Gets the collection of validation errors.
        /// </summary>
        public IReadOnlyCollection<string> Errors => new ReadOnlyCollection<string>(this.errors);

        /// <summary>
        /// Gets whether there are any errors.
        /// </summary>
        public bool HasErrors => this.errors.Any();

        /// <summary>
        /// Add a new error message.
        /// </summary>
        /// <param name="theErrorMessage">Error message.</param>
        public void AddError(string theErrorMessage)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theErrorMessage));
            Contract.Assume(this.errors != null);
            this.errors.Add(theErrorMessage);
        }
    }
}
