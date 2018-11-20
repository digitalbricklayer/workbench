using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a variable is deleted from the model.
    /// </summary>
    public class VariableDeletedMessage : WorkspaceChangedMessage
    {
        /// <summary>
        /// Initialize a variable deleted message with the deleted variable.
        /// </summary>
        /// <param name="theDeletedVariable">The variable that has been deleted.</param>
        public VariableDeletedMessage(VariableModel theDeletedVariable)
        {
            Contract.Requires<ArgumentNullException>(theDeletedVariable != null);
            DeletedVariable = theDeletedVariable;
        }

        /// <summary>
        /// Gets the name of the deleted variable.
        /// </summary>
        public string VariableName => DeletedVariable.Name;

        /// <summary>
        /// Gets the variable that has been deleted.
        /// </summary>
        public VariableModel DeletedVariable { get; private set; }
    }
}
