using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// An encapsulated variable is used in binarization of ternary expressions.
    /// </summary>
    internal sealed class EncapsulatedVariable
    {
        /// <summary>
        /// Initialize an encapsulated variable with a variable name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        internal EncapsulatedVariable(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            VariableName = variableName;
        }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        internal string VariableName { get; }
    }
}
