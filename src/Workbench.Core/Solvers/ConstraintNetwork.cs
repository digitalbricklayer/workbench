using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A constraint network made up of arcs.
    /// </summary>
    internal sealed class ConstraintNetwork
    {
        private readonly List<Arc> _arcs;
        private readonly List<IntegerVariable> _singletonVariables;
        private readonly List<AggregateIntegerVariable> _aggregateVariables;

        /// <summary>
        /// Initialize a constraint network with default values.
        /// </summary>
        internal ConstraintNetwork()
        {
            _arcs = new List<Arc>();
            _singletonVariables = new List<IntegerVariable>();
            _aggregateVariables = new List<AggregateIntegerVariable>();
        }

        /// <summary>
        /// Gets whether the constraint network has been solved successfully.
        /// </summary>
        internal bool IsSolved
        {
            get { return _arcs.TrueForAll(arc => arc.IsSolved); }
        }

        /// <summary>
        /// Get all of the arcs in the constraint network.
        /// </summary>
        /// <returns>Array of all of the arcs.</returns>
        internal Arc[] ToArray()
        {
            return _arcs.ToArray();
        }

        internal void AddVariable(IntegerVariable variable)
        {
            _singletonVariables.Add(variable);
        }

        internal void AddVariable(AggregateIntegerVariable variable)
        {
            _aggregateVariables.Add(variable);
        }

        /// <summary>
        /// Add an arc to the network.
        /// </summary>
        /// <param name="arc">Arc to add.</param>
        internal void AddArc(Arc arc)
        {
            _arcs.Add(arc);
        }

        /// <summary>
        /// Get all singleton variables inside the constraint network.
        /// </summary>
        /// <returns>All singleton variables in a read only collection.</returns>
        internal IReadOnlyCollection<IntegerVariable> GetSingletonVariables()
        {
            return _singletonVariables.AsReadOnly();
        }

        /// <summary>
        /// Get all aggregate variables inside the constraint network.
        /// </summary>
        /// <returns>All aggregate variables in a read only collection.</returns>
        internal IReadOnlyCollection<AggregateIntegerVariable> GetAggregateVariables()
        {
            return _aggregateVariables.AsReadOnly();
        }
    }
}
