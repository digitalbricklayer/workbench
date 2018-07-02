using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a new singleton variable is added to the model.
    /// </summary>
    public class SingletonVariableAddedMessage
    {
        /// <summary>
        /// Initialize a new singleton variable added message with the new variable.
        /// </summary>
        /// <param name="theNewVariable">New variable.</param>
        public SingletonVariableAddedMessage(SingletonVariableEditorViewModel theNewVariable)
        {
            Contract.Requires<ArgumentNullException>(theNewVariable != null);
            NewVariable = theNewVariable;
        }

        /// <summary>
        /// Gets the new variable.
        /// </summary>
        public SingletonVariableEditorViewModel NewVariable { get; private set; }

        /// <summary>
        /// Gets the new variable name.
        /// </summary>
        public string NewVariableName
        {
            get
            {
                Contract.Assume(NewVariable != null);
                return NewVariable.Name;
            }
        }
    }
}
