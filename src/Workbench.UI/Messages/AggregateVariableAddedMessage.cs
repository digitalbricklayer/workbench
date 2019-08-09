using Workbench.Core.Models;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a new aggregate variable is added to the model.
    /// </summary>
    public class AggregateVariableAddedMessage : WorkspaceChangedMessage
    {
        /// <summary>
        /// Initialize the new aggregate added message with the new aggregate variable.
        /// </summary>
        /// <param name="newVariable"></param>
        public AggregateVariableAddedMessage(AggregateVariableModel newVariable)
        {
            Added = newVariable;
        }

        /// <summary>
        /// Gets the new aggregate variable.
        /// </summary>
        public AggregateVariableModel Added { get; private set; }

        /// <summary>
        /// Gets the new aggregate variable name.
        /// </summary>
        public string NewVariableName
        {
            get
            {
                return Added.Name;
            }
        }
    }
}
