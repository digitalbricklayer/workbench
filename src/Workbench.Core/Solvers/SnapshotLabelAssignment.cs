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
        private readonly List<VariableBase> _unassignedVariables;

        internal SnapshotLabelAssignment(IEnumerable<VariableBase> allVariables)
        {
            _assignments = new Dictionary<string, LabelAssignment>();
            _unassignedVariables = new List<VariableBase>(allVariables);
        }

        internal void AssignTo(ValueSet valueSet)
        {
            foreach (var variableValue in valueSet.Values)
            {
                var solverVariable = variableValue.Variable;
                AssignTo(solverVariable, variableValue.Content);
            }
        }

        internal void AssignTo(VariableBase variable, int value)
        {
            _assignments[variable.Name] = new LabelAssignment(variable, value);
            _unassignedVariables.Remove(variable);
        }

        internal void Remove(VariableBase variable)
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

        internal bool IsAssigned(VariableBase variable)
        {
            return _assignments.ContainsKey(variable.Name);
        }

        internal LabelAssignment GetAssignmentFor(VariableBase variable)
        {
            if (!_assignments.ContainsKey(variable.Name))
                throw new ArgumentException("Variable has no label assignment.", nameof(variable));
            return _assignments[variable.Name];
        }

        public void Remove(List<Value> inconsistentValues)
        {
            foreach (var variableValue in inconsistentValues)
            {
                var solverVariable = variableValue.Variable;
                Remove(solverVariable);
            }
        }
    }
}
