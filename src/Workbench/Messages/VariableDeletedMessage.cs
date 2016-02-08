using System;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a variable is deleted.
    /// </summary>
    public class VariableDeletedMessage
    {
        /// <summary>
        /// Initialize a variable deleted message with the name of the deleted variable.
        /// </summary>
        /// <param name="variableDeleted">Name of the deleted variable.</param>
        public VariableDeletedMessage(string variableDeleted)
        {
            if (string.IsNullOrWhiteSpace(variableDeleted))
                throw new ArgumentException("variableDeleted");
            this.VariableName = variableDeleted;
        }

        /// <summary>
        /// Gets the name of the deleted variable.
        /// </summary>
        public string VariableName { get; private set; }
    }
}
