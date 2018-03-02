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
        /// Initialize a variable deleted message with the deleted variable.
        /// </summary>
        /// <param name="theDeletedVariable">The variable that has been deleted.</param>
        public VariableDeletedMessage(VariableVisualizerViewModel theDeletedVariable)
        {
            Contract.Requires<ArgumentNullException>(theDeletedVariable != null);
            Deleted = theDeletedVariable;
        }

        /// <summary>
        /// Gets the name of the deleted variable.
        /// </summary>
        public string VariableName => Deleted.Name;

        /// <summary>
        /// Gets the variable that has been deleted.
        /// </summary>
        public VariableVisualizerViewModel Deleted { get; private set; }
    }
}
