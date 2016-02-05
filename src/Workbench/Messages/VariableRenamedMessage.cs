using System;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when a variable has been renamed.
    /// </summary>
    public class VariableRenamedMessage
    {
        /// <summary>
        /// Initialize a new variable renamed message with the old variable name and the new variable name.
        /// </summary>
        /// <param name="theOldName">Old variable name.</param>
        /// <param name="theNewName">New variable name.</param>
        public VariableRenamedMessage(string theOldName, string theNewName)
        {
            if (string.IsNullOrWhiteSpace(theOldName))
                throw new ArgumentException("Invalid old name.", "theOldName");

            if (string.IsNullOrWhiteSpace(theNewName))
                throw new ArgumentException("Invalid new name.", "theNewName");

            this.OldName = theOldName;
            this.NewName = theNewName;
        }

        public string OldName { get; private set; }
        public string NewName { get; private set; }
    }
}
