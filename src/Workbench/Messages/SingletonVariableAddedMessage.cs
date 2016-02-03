using System.Diagnostics;
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
        public SingletonVariableAddedMessage(VariableViewModel theNewVariable)
        {
            NewVariable = theNewVariable;
        }

        /// <summary>
        /// Gets the new variable.
        /// </summary>
        public VariableViewModel NewVariable { get; private set; }

        /// <summary>
        /// Gets the new variable name.
        /// </summary>
        public string NewVariableName
        {
            get
            {
                Debug.Assert(this.NewVariable != null);
                return this.NewVariable.Name;
            }
        }
    }
}
