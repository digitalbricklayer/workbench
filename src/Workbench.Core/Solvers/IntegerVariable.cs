using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Integer variable used internally by the solver.
    /// </summary>
    internal sealed class IntegerVariable
    {
        /// <summary>
        /// Gets the domain range.
        /// </summary>
        internal DomainRange Domain { get; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Initialize an integer variable with a name and range.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="range">Domain range.</param>
        internal IntegerVariable(string name, DomainRange range)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            Name = name;
            Domain = range;
        }
    }
}