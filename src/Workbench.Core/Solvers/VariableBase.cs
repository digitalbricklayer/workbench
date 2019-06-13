using System;
using System.Diagnostics.Contracts;

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
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            Name = variableName;
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        internal string Name { get; }
    }
}