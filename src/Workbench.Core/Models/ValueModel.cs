using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A value that is bound to a variable.
    /// </summary>
    [Serializable]
    public sealed class ValueModel
    {
        /// <summary>
        /// Initialize a value binding with a model value.
        /// </summary>
        /// <param name="theModelValue">Model value.</param>
        public ValueModel(object theModelValue)
        {
            Contract.Requires<ArgumentNullException>(theModelValue != null);

            Model = theModelValue;
        }

        /// <summary>
        /// Gets the model value.
        /// </summary>
        public object Model { get; private set; }
    }
}
