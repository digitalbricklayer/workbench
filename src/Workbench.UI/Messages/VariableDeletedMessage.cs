using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a variable is deleted from the model.
    /// </summary>
    public class VariableDeletedMessage
    {
        /// <summary>
        /// Initialize a variable deleted message with the name of the deleted variable.
        /// </summary>
        /// <param name="theDeletedVariable">The variable that has been deleted.</param>
        public VariableDeletedMessage(VariableViewModel theDeletedVariable)
        {
            Contract.Requires<ArgumentNullException>(theDeletedVariable != null);
            this.Deleted = theDeletedVariable;
        }

        /// <summary>
        /// Gets the name of the deleted variable.
        /// </summary>
        public string VariableName
        {
            get { return this.Deleted.Name; }
        }

        /// <summary>
        /// Gets the variable that has been deleted.
        /// </summary>
        public VariableViewModel Deleted { get; private set; }
    }
}
