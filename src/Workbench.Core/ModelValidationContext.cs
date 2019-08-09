using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            if (string.IsNullOrWhiteSpace(theErrorMessage))
                throw new ArgumentException(nameof(theErrorMessage));
            Debug.Assert(this.errors != null);
            this.errors.Add(theErrorMessage);
        }
    }
}
