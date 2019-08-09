using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A constraint network made up of arcs.
    /// </summary>
    internal sealed class ConstraintNetwork
    {
        private readonly List<Arc> _arcs;
        private readonly List<SolverVariable> _singletonVariables;
        private readonly List<AggregateSolverVariable> _aggregateVariables;
        private readonly List<OrangeBucketVariableMap> _bucketMaps;

        /// <summary>
        /// Initialize a constraint network with default values.
        /// </summary>
        internal ConstraintNetwork()
        {
            _arcs = new List<Arc>();
            _singletonVariables = new List<SolverVariable>();
            _aggregateVariables = new List<AggregateSolverVariable>();
            _bucketMaps = new List<OrangeBucketVariableMap>();
        }

        /// <summary>
        /// Gets whether the constraint network has been solved successfully.
        /// </summary>
        internal bool IsArcConsistent()
        {
            return _arcs.TrueForAll(arc => arc.IsArcConsistent());
        }

        /// <summary>
        /// Get all of arcs in the constraint network.
        /// </summary>
        /// <returns>Array of all of the arcs.</returns>
        internal IReadOnlyCollection<Arc> GetArcs()
        {
            return _arcs.AsReadOnly();
        }

        internal void AddVariable(SolverVariable variable)
        {
            _singletonVariables.Add(variable);
        }

        internal void AddVariable(AggregateSolverVariable variable)
        {
            _aggregateVariables.Add(variable);
        }

        internal void AddVariable(OrangeBucketVariableMap variable)
        {
            _bucketMaps.Add(variable);
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
        /// Add an arc to the network.
        /// </summary>
        /// <param name="arcs">Arcs to add.</param>
        internal void AddArc(IEnumerable<Arc> arcs)
        {
            _arcs.AddRange(arcs);
        }

        /// <summary>
        /// Get all singleton variables inside the constraint network.
        /// </summary>
        /// <returns>All singleton variables in a read only collection.</returns>
        internal IReadOnlyCollection<VariableBase> GetVariables()
        {
            var allVariablesIncEncapsulated = new List<VariableBase>();

            foreach (var arc in _arcs)
            {
                if (arc.IsPreComputed)
                {
                    if (arc.Left.Content is EncapsulatedVariable leftEncapsulatedVariable)
                    {
                        if (!allVariablesIncEncapsulated.Contains(leftEncapsulatedVariable))
                        {
                            allVariablesIncEncapsulated.Add(leftEncapsulatedVariable);
                        }
                    }

                    if (arc.Right.Content is EncapsulatedVariable rightEncapsulatedVariable)
                    {
                        if (!allVariablesIncEncapsulated.Contains(rightEncapsulatedVariable))
                        {
                            allVariablesIncEncapsulated.Add(rightEncapsulatedVariable);
                        }
                    }
                }
            }

            allVariablesIncEncapsulated.AddRange(_singletonVariables);
            foreach (var aggregateSolverVariable in _aggregateVariables)
            {
                foreach (var solverVariable in aggregateSolverVariable.Variables)
                {
                    allVariablesIncEncapsulated.Add(solverVariable);
                }
            }

            foreach (var bucketMap in _bucketMaps)
            {
                foreach (var bundleMap in bucketMap.GetBundleMaps())
                {
                    foreach (var variableMap in bundleMap.GetVariableMaps())
                    {
                        allVariablesIncEncapsulated.Add(variableMap.SolverVariable);
                    }
                }
            }

            return allVariablesIncEncapsulated;
        }

        /// <summary>
        /// Get all singleton variables inside the constraint network.
        /// </summary>
        /// <returns>All singleton variables in a read only collection.</returns>
        internal IReadOnlyCollection<SolverVariable> GetSingletonVariables()
        {
            return _singletonVariables.AsReadOnly();
        }

        /// <summary>
        /// Get all aggregate variables inside the constraint network.
        /// </summary>
        /// <returns>All aggregate variables in a read only collection.</returns>
        internal IReadOnlyCollection<AggregateSolverVariable> GetAggregateVariables()
        {
            return _aggregateVariables.AsReadOnly();
        }

        public IReadOnlyCollection<SolverVariable> GetSolverVariables()
        {
            var allVariablesIncEncapsulated = new List<SolverVariable>();

            allVariablesIncEncapsulated.AddRange(_singletonVariables);
            foreach (var aggregateSolverVariable in _aggregateVariables)
            {
                foreach (var solverVariable in aggregateSolverVariable.Variables)
                {
                    allVariablesIncEncapsulated.Add(solverVariable);
                }
            }

            foreach (var bucketMap in _bucketMaps)
            {
                foreach (var bundleMap in bucketMap.GetBundleMaps())
                {
                    foreach (var variableMap in bundleMap.GetVariableMaps())
                    {
                        allVariablesIncEncapsulated.Add(variableMap.SolverVariable);
                    }
                }
            }

            return allVariablesIncEncapsulated.AsReadOnly();
        }
    }
}
