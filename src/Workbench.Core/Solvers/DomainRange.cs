using System.Collections.Generic;
using System.Linq;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Fully expended range of values in a domain.
    /// </summary>
    internal sealed class DomainRange
    {
        private readonly List<int> _possibleValues;

        /// <summary>
        /// Initialize a domain range with all possible values.
        /// </summary>
        /// <param name="allPossibleValues">All values a variable can be bound to.</param>
        internal DomainRange(int[] allPossibleValues)
        {
            _possibleValues = new List<int>(allPossibleValues);
        }

        /// <summary>
        /// Gets whether the range is empty.
        /// </summary>
        internal bool IsEmpty => !_possibleValues.Any();

        /// <summary>
        /// Gets all of the possible values.
        /// </summary>
        internal IReadOnlyCollection<int> PossibleValues => _possibleValues.AsReadOnly();

        /// <summary>
        /// Remove a possible value from the range.
        /// </summary>
        /// <param name="value">Value to be removed.</param>
        internal void Remove(int value)
        {
            _possibleValues.Remove(value);
        }
    }
}