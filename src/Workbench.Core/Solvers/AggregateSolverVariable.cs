using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Aggregate Integer variable used internally by the solver.
    /// </summary>
    internal sealed class AggregateSolverVariable : SolverVariable
    {
        private readonly SolverVariable[] _variables;

        /// <summary>
        /// Gets all of the internal variables.
        /// </summary>
        internal IReadOnlyCollection<SolverVariable> Variables => new ReadOnlyCollection<SolverVariable>(_variables);

        /// <summary>
        /// Initialize an aggregate integer variable with a name, size and domain.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="size">Size of the aggregate.</param>
        /// <param name="domain">Aggregate domain.</param>
        internal AggregateSolverVariable(string variableName, int size, DomainRange domain)
            : base(variableName, domain)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentException("Aggregate variable name cannot be empty", nameof(variableName));

            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            _variables = CreateVariables(size);
        }

        /// <summary>
        /// Get the variable at the index.
        /// </summary>
        /// <param name="index">Index of the variable.</param>
        /// <returns>Integer variable.</returns>
        internal SolverVariable GetAt(int index)
        {
            if (index < 0 || index >= _variables.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _variables[index];
        }

        private SolverVariable[] CreateVariables(int size)
        {
            var internalVariables = new List<SolverVariable>();

            // Populate the internal variables
            for (var i = 0; i < size; i++)
            {
                internalVariables.Add(new SolverVariable(GetVariableNameFor(i), CloneFrom(Domain)));
            }

            return internalVariables.ToArray();
        }

        /// <summary>
        /// Get the variable name for the variable at the index.
        /// </summary>
        /// <param name="index">Index the variable.</param>
        /// <returns>Variable name.</returns>
        private string GetVariableNameFor(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            Debug.Assert(!string.IsNullOrWhiteSpace(Name));

            return Name + (index + 1);
        }

        private DomainRange CloneFrom(DomainRange template)
        {
            return new DomainRange(template.PossibleValues.ToArray());
        }
    }
}
