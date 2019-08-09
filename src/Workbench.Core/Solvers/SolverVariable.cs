using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Integer variable used internally by the solver.
    /// </summary>
    internal class SolverVariable : VariableBase
    {
        /// <summary>
        /// Initialize a solver variable with a name and range.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="range">Domain range.</param>
        internal SolverVariable(string name, DomainRange range)
            : base(name)
        {
            Domain = range;
        }

        /// <summary>
        /// Gets the domain range.
        /// </summary>
        internal DomainRange Domain { get; }

        /// <summary>
        /// Get all values that can be assigned to the variable.
        /// </summary>
        /// <returns>All values that can be assigned to the variable.</returns>
        internal IEnumerable<ValueSet> GetCandidates()
        {
            var valueBindingAccumulator = new List<ValueSet>();
            foreach (var possibleValue in Domain.PossibleValues)
            {
                valueBindingAccumulator.Add(new ValueSet(new Value(this, possibleValue)));
            }
            return valueBindingAccumulator.AsReadOnly();
        }
    }
}
