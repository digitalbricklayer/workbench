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
        private readonly List<VariableBase> _variables;
        private readonly List<ValueSet> _assignments;

        internal SnapshotLabelAssignment(IEnumerable<VariableBase> allVariables)
        {
            _assignments = new List<ValueSet>();
            _variables = new List<VariableBase>(allVariables);
        }

        internal void AssignTo(ValueSet valueSet)
        {
            _assignments.Add(valueSet);
        }

        internal bool Remove(ValueSet valueSet)
        {
            return _assignments.Remove(valueSet);
        }

        internal bool IsComplete()
        {
            return _variables.All(IsAssigned);
        }

        internal bool IsAssigned(VariableBase variable)
        {
            return _assignments.Any(valueSet => valueSet.IsAssigned(variable.Name));
        }

        internal LabelAssignment GetAssignmentFor(VariableBase variable)
        {
            if (!_assignments.Any(set => set.IsAssigned(variable.Name)))
                throw new ArgumentException("Variable has no label assignment.", nameof(variable));
            var valueSet = _assignments.First(set => set.IsAssigned(variable.Name));
            return new LabelAssignment(variable, valueSet.GetValueFor(variable.Name));
        }
    }
}
