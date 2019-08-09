using Workbench.Core.Models;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a variable has been renamed.
    /// </summary>
    public class VariableRenamedMessage : WorkspaceChangedMessage
    {
        /// <summary>
        /// Initialize a new variable renamed message with the previous variable name and the 
        /// variable that has been renamed.
        /// </summary>
        /// <param name="thePreviousName">Previous variable name.</param>
        /// <param name="theVariable">The renamed variable.</param>
        public VariableRenamedMessage(ModelName thePreviousName, VariableModel theVariable)
        {
            PreviousName = thePreviousName.Text;
            Renamed = theVariable;
        }

        /// <summary>
        /// Gets the previous variable name.
        /// </summary>
        public string PreviousName { get; private set; }

        /// <summary>
        /// Gets the variable that was renamed.
        /// </summary>
        public VariableModel Renamed { get; private set; }
    }
}
