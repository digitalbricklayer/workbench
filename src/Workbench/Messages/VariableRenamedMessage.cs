using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a variable has been renamed.
    /// </summary>
    public class VariableRenamedMessage
    {
        /// <summary>
        /// Initialize a new variable renamed message with the old variable name and the 
        /// variable that has been renamed.
        /// </summary>
        /// <param name="theOldName">Old variable name.</param>
        /// <param name="theVariable">The renamed variable.</param>
        public VariableRenamedMessage(string theOldName, VariableViewModel theVariable)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theOldName));
            Contract.Requires<ArgumentNullException>(theVariable != null);

            this.OldName = theOldName;
            this.Renamed = theVariable;
        }

        /// <summary>
        /// Gets the old variable name.
        /// </summary>
        public string OldName { get; private set; }

        /// <summary>
        /// Gets the variable that was renamed.
        /// </summary>
        public VariableViewModel Renamed { get; private set; }
    }
}
