using System.Collections.Generic;
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
        internal IReadOnlyCollection<ValueSet> GetSets()
        {
            return _sets.AsReadOnly();
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

        internal void RemoveSets(IReadOnlyList<ValueSet> valueSetsToRemove)
        {
            foreach (var valueSetToRemove in valueSetsToRemove)
            {
                _sets.Remove(valueSetToRemove);
            }
        }
    }
}
