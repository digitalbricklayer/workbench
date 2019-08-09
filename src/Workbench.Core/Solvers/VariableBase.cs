using System;

namespace Workbench.Core.Solvers
{
    internal abstract class VariableBase
    {
        /// <summary>
        /// Initialize an encapsulated variable with a variable name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        protected VariableBase(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentException("Variable name must not be blank", nameof(variableName));

            Name = variableName;
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        internal string Name { get; }
    }
}