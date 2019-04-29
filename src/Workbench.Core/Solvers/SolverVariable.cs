using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Integer variable used internally by the solver.
    /// </summary>
    internal class SolverVariable
    {
        /// <summary>
        /// Initialize a solver variable with a name and range.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="range">Domain range.</param>
        internal SolverVariable(string name, DomainRange range)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            Contract.Requires<ArgumentException>(range != null);
            Name = name;
            Domain = range;
        }

        /// <summary>
        /// Gets the domain range.
        /// </summary>
        internal DomainRange Domain { get; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Get all values that can be assigned to the variable.
        /// </summary>
        /// <returns>All values that can be assigned to the variable.</returns>
        internal IEnumerable<int> GetCandidates()
        {
            return Domain.PossibleValues;
        }
    }
}
