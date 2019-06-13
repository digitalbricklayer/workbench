using System;
using System.Collections.Generic;
using System.Linq;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Captures snapshot variable assignment state.     
    /// </summary>
    internal sealed class SnapshotLabelAssignment
    {
        private readonly Dictionary<string, LabelAssignment> _assignments;
        private readonly List<SolverVariable> _unassignedVariables;

        internal SnapshotLabelAssignment(IEnumerable<SolverVariable> allVariables)
        {
            _assignments = new Dictionary<string, LabelAssignment>();
            _unassignedVariables = new List<SolverVariable>(allVariables);
        }

        internal void AssignTo(SolverVariable variable, int value)
        {
#if false
            if (_assignments.ContainsKey(variable.Name))
                throw new ArgumentException("Variable already has a label assignment.", nameof(variable));
#endif
            _assignments[variable.Name] = new LabelAssignment(variable, value);
            _unassignedVariables.Remove(variable);
        }

        internal void Remove(SolverVariable variable)
        {
            if (!_assignments.ContainsKey(variable.Name))
                throw new ArgumentException("Variable has no label assignment.", nameof(variable));
            _assignments.Remove(variable.Name);
            _unassignedVariables.Add(variable);
        }

        internal bool IsComplete()
        {
            return !_unassignedVariables.Any();
        }

        internal bool IsAssigned(SolverVariable variable)
        {
            return _assignments.ContainsKey(variable.Name);
        }

        public LabelAssignment GetAssignmentFor(SolverVariable variable)
        {
            if (!_assignments.ContainsKey(variable.Name))
                throw new ArgumentException("Variable has no label assignment.", nameof(variable));
            return _assignments[variable.Name];
        }
    }
}