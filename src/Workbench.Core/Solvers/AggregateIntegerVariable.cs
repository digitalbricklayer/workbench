using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Aggregate Integer variable used internally by the solver.
    /// </summary>
    internal sealed class AggregateIntegerVariable
    {
        private readonly IntegerVariable[] _variables;

        /// <summary>
        /// Gets the aggregate variable name.
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Gets the domain range.
        /// </summary>
        internal DomainRange Domain { get; }

        /// <summary>
        /// Gets all of the internal variables.
        /// </summary>
        internal IReadOnlyCollection<IntegerVariable> Variables => new ReadOnlyCollection<IntegerVariable>(_variables);

        /// <summary>
        /// Initialize an aggregate integer variable with a name, size and domain.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <param name="size">Size of the aggregate.</param>
        /// <param name="domain">Aggregate domain.</param>
        internal AggregateIntegerVariable(string variableName, int size, DomainRange domain)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(variableName));
            Contract.Requires<ArgumentOutOfRangeException>(size > 0);

            Name = variableName;
            Domain = domain;
            _variables = CreateVariables(size);
        }

        /// <summary>
        /// Get the variable at the index.
        /// </summary>
        /// <param name="index">Index of the variable.</param>
        /// <returns>Integer variable.</returns>
        internal IntegerVariable GetAt(int index)
        {
            if (index < 0 || index >= _variables.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _variables[index];
        }

        private IntegerVariable[] CreateVariables(int size)
        {
            var internalVariables = new List<IntegerVariable>();

            // Populate the internal variables
            for (var i = 0; i < size; i++)
            {
                internalVariables.Add(new IntegerVariable(GetVariableNameFor(i), CloneFrom(Domain)));
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
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);
            Contract.Assume(!string.IsNullOrWhiteSpace(Name));

            return Name + index;
        }

        private DomainRange CloneFrom(DomainRange template)
        {
            return new DomainRange(template.PossibleValues.ToArray());
        }
    }
}
