using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Collection of value sets assigned to an encapsulated variable.
    /// </summary>
    internal sealed class EncapsulatedVariableDomainValue
    {
        private readonly List<ValueSet> _sets;
        private readonly EncapsulatedVariable _encapsulatedVariable;

        /// <summary>
        /// Initialize an encapsulated variable domain value with an encapsulated variable and value sets.
        /// </summary>
        internal EncapsulatedVariableDomainValue(EncapsulatedVariable encapsulatedVariable, IEnumerable<ValueSet> valueSets)
        {
            _encapsulatedVariable = encapsulatedVariable;
            _sets = new List<ValueSet>(valueSets);
        }

        /// <summary>
        /// Gets whether the union is empty.
        /// </summary>
        internal bool IsEmpty => !_sets.Any();

        /// <summary>
        /// Get all value sets.
        /// </summary>
        /// <returns>All value sets.</returns>
        internal IEnumerable<ValueSet> GetSets()
        {
            return new ReadOnlyCollection<ValueSet>(_sets);
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.Append("{");
            var i = 1;
            foreach (var set in _sets)
            {
                output.Append(set);
                if (i < _sets.Count)
                    output.Append(",");
                i++;
            }
            output.Append("}");

            return output.ToString();
        }
    }

    /// <summary>
    /// A set of values that may be assigned to one or more variables.
    /// </summary>
    internal sealed class ValueSet
    {
        private readonly List<int> _values;

        internal ValueSet(IEnumerable<int> variableValues)
        {
            Contract.Requires<ArgumentNullException>(variableValues != null);

            _values = new List<int>(variableValues);
        }

        internal int GetAt(int index)
        {
            if (index < 0 || index >= _values.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _values[index];
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.Append('(');

            var i = 1;
            foreach (var value in _values)
            {
                output.Append(value);
                if (i < _values.Count)
                    output.Append(",");
                i++;
            }

            output.Append(')');

            return output.ToString();
        }

    }
}
