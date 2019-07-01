using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Extracts a solution snapshot from the orange problem representation.
    /// </summary>
    internal sealed class OrangeSnapshotExtractor
    {
        private readonly OrangeModelSolverMap _modelSolverMap;
        private readonly OrangeValueMapper _valueMapper;
        private List<VariableBase> _variables;
        private ConstraintNetwork _constraintNetwork;

        /// <summary>
        /// Initialize a snapshot extractor with a model solver map and value mapper.
        /// </summary>
        /// <param name="modelSolverMap">Map between the model and solver representations.</param>
        /// <param name="valueMapper">Map between the model and solver values.</param>
        internal OrangeSnapshotExtractor(OrangeModelSolverMap modelSolverMap, OrangeValueMapper valueMapper)
        {
            _modelSolverMap = modelSolverMap;
            _valueMapper = valueMapper;
        }

        /// <summary>
        /// Extract a snapshot from the constraint network.
        /// </summary>
        /// <param name="constraintNetwork">Constraint network.</param>
        /// <param name="solutionSnapshot">Solution snapshot.</param>
        /// <returns>True if a snapshot was extracted, False if the snapshot could not be extracted.</returns>
        internal bool ExtractFrom(ConstraintNetwork constraintNetwork, out SolutionSnapshot solutionSnapshot)
        {
            _constraintNetwork = constraintNetwork;
            return BacktrackingSearch(constraintNetwork, out solutionSnapshot);
        }

        private bool BacktrackingSearch(ConstraintNetwork constraintNetwork, out SolutionSnapshot solutionSnapshot)
        {
            var solverVariables = constraintNetwork.GetSolverVariables();
            var assignment = new SnapshotLabelAssignment(solverVariables);
            _variables = new List<VariableBase>(constraintNetwork.GetVariables());
            var status = Backtrack(0, assignment, constraintNetwork);

            solutionSnapshot = status ? ConvertSnapshotFrom(assignment, constraintNetwork) : SolutionSnapshot.Empty;

            return status;
        }

        private bool Backtrack(int currentVariableIndex, SnapshotLabelAssignment snapshotAssignment, ConstraintNetwork constraintNetwork)
        {
            // Label assignment has been successful...
            if (snapshotAssignment.IsComplete() && AllVariablesTested(currentVariableIndex)) return true;

            // The unassigned variable may be a regular variable or an encapsulated variable
            var currentVariable = SelectUnassignedVariable(currentVariableIndex, constraintNetwork, snapshotAssignment);

            Debug.Assert(currentVariable != null, "Snapshot is not complete so there must be more variables.");

            foreach (var value in OrderDomainValues(currentVariable, snapshotAssignment, constraintNetwork))
            {
                if (IsConsistent(value, snapshotAssignment))
                {
                    snapshotAssignment.AssignTo(value);

                    // Is this the final variable?
                    if (AllVariablesTested(currentVariableIndex)) return true;

                    if (Backtrack(currentVariableIndex + 1, snapshotAssignment, constraintNetwork))
                    {
                        return true;
                    }
                }

                snapshotAssignment.Remove(value);
            }

            return false;
        }

        private IEnumerable<ValueSet> OrderDomainValues(VariableBase variable, SnapshotLabelAssignment assignment, ConstraintNetwork constraintNetwork)
        {
            switch (variable)
            {
                case SolverVariable solverVariable:
                    return solverVariable.GetCandidates();

                case EncapsulatedVariable encapsulatedVariable:
                    return encapsulatedVariable.GetCandidates();

                default:
                    throw new NotImplementedException();
            }
        }

        private bool IsConsistent(ValueSet valueSet, SnapshotLabelAssignment assignment)
        {
            /*
             * All variables in the set must be consistent, either not having been assigned
             * a value or having a value that is consistent with the currently assigned value.
             */
            var inconsistentValueAccumulator = valueSet.Values.Where(value => !IsConsistent(value, assignment))
                                                              .ToList();
            return !inconsistentValueAccumulator.Any();
        }

        private bool IsConsistent(Value value, SnapshotLabelAssignment assignment)
        {
            var variable = value.Variable;

            // Has the variable been assigned a value? If it has not, then the value must be consistent
            if (!assignment.IsAssigned(variable)) return true;

            var labelAssignment = assignment.GetAssignmentFor(variable);

            /*
             * The variable has been assigned a value, it is consistent if the
             * value is the same as the assigned value.
             */
            return labelAssignment.Value == value.Content;
        }

        private bool AllVariablesTested(int variableIndex)
        {
            return variableIndex + 1 >=_constraintNetwork.GetVariables().Count;
        }

        private VariableBase SelectUnassignedVariable(int currentVariableIndex, ConstraintNetwork constraintNetwork, SnapshotLabelAssignment assignment)
        {
            Debug.Assert(currentVariableIndex < _variables.Count, "Backtracking must never attempt to go over the end of the variables.");

            return _variables[currentVariableIndex];
        }

        private SolutionSnapshot ConvertSnapshotFrom(SnapshotLabelAssignment assignment, ConstraintNetwork constraintNetwork)
        {
            return new SolutionSnapshot(ExtractSingletonLabelsFrom(assignment, constraintNetwork),
                                        ExtractAggregateLabelsFrom(assignment, constraintNetwork));
        }

        private IEnumerable<SingletonVariableLabelModel> ExtractSingletonLabelsFrom(SnapshotLabelAssignment assignment, ConstraintNetwork constraintNetwork)
        {
            var labelAccumulator = new List<SingletonVariableLabelModel>();
            var allSingletonVariables = constraintNetwork.GetSingletonVariables();

            foreach (var variable in allSingletonVariables)
            {
                var solverVariable = _modelSolverMap.GetSolverSingletonVariableByName(variable.Name);
                var labelAssignment = assignment.GetAssignmentFor(solverVariable);
                var variableSolverValue = labelAssignment.Value;
                var variableModel = _modelSolverMap.GetModelSingletonVariableByName(variable.Name);
                var variableModelValue = ConvertSolverValueToModel(variableModel, variableSolverValue);
                var label = new SingletonVariableLabelModel(variableModel, new ValueModel(variableModelValue));
                labelAccumulator.Add(label);
            }

            return labelAccumulator;
        }

        private IEnumerable<AggregateVariableLabelModel> ExtractAggregateLabelsFrom(SnapshotLabelAssignment assignment, ConstraintNetwork constraintNetwork)
        {
            var aggregatorLabelAccumulator = new List<AggregateVariableLabelModel>();
            var allAggregateVariables = constraintNetwork.GetAggregateVariables();

            foreach (var aggregateSolverVariable in allAggregateVariables)
            {
                var internalAccumulator = new List<ValueModel>();
                var aggregateVariableModel = _modelSolverMap.GetModelAggregateVariableByName(aggregateSolverVariable.Name);
                foreach (var variable in aggregateSolverVariable.Variables)
                {
                    var solverVariable = _modelSolverMap.GetSolverVariableByName(variable.Name);
                    var labelAssignment = assignment.GetAssignmentFor(solverVariable);
                    var variableSolverValue = labelAssignment.Value;
                    var variableModel = _modelSolverMap.GetInternalModelAggregateVariableByName(variable.Name);
                    var variableModelValue = ConvertSolverValueToModel(variableModel, variableSolverValue);
                    internalAccumulator.Add(new ValueModel(variableModelValue));
                }

                var label = new AggregateVariableLabelModel(aggregateVariableModel, internalAccumulator);
                aggregatorLabelAccumulator.Add(label);
            }

            return aggregatorLabelAccumulator;
        }

        private object ConvertSolverValueToModel(VariableModel theVariable, long solverValue)
        {
            var variableDomainValue = _valueMapper.GetDomainValueFor(theVariable);
            return variableDomainValue.MapFrom(solverValue);
        }
    }
}
