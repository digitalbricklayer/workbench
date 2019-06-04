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
        private readonly ValueMapper _valueMapper;
        private IEnumerator<SolverVariable> _variableEnumerator;

        /// <summary>
        /// Initialize a snapshot extractor with a model solver map and value mapper.
        /// </summary>
        /// <param name="modelSolverMap">Map between the model and solver representations.</param>
        /// <param name="valueMapper">Map between the model and solver values.</param>
        internal OrangeSnapshotExtractor(OrangeModelSolverMap modelSolverMap, ValueMapper valueMapper)
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
#if false
            solutionSnapshot = new SolutionSnapshot(ExtractSingletonLabelsFrom(constraintNetwork), 
                                                    ExtractAggregateLabelsFrom(constraintNetwork));

            return true;
#else
            return BacktrackingSearch(constraintNetwork, out solutionSnapshot);
#endif
        }

        private bool BacktrackingSearch(ConstraintNetwork constraintNetwork, out SolutionSnapshot solutionSnapshot)
        {
            var assignment = new SnapshotLabelAssignment(constraintNetwork.GetSingletonVariables());
            _variableEnumerator = constraintNetwork.GetSingletonVariables().GetEnumerator();
            var status = Backtrack(assignment, constraintNetwork);

            solutionSnapshot = status ? ConvertSnapshotFrom(assignment, constraintNetwork) : SolutionSnapshot.Empty;

            return status;
        }

        private bool Backtrack(SnapshotLabelAssignment snapshotAssignment, ConstraintNetwork constraintNetwork)
        {
            // Label assignment has been successful...
            if (snapshotAssignment.IsComplete()) return true;

            var currentVariable = SelectUnassignedVariable(constraintNetwork, snapshotAssignment);
            Debug.Assert(currentVariable != null, "Snapshot is not complete so there must be more variables.");
            foreach (var value in OrderDomainValues(currentVariable, snapshotAssignment, constraintNetwork))
            {
                if (IsConsistent(value, snapshotAssignment, currentVariable))
                {
                    snapshotAssignment.AssignTo(currentVariable, value);
                    var status = Backtrack(snapshotAssignment, constraintNetwork);
                    if (status)
                    {
                        return true;
                    }
                }

                snapshotAssignment.Remove(currentVariable);
            }

            return false;
        }

        private IEnumerable<int> OrderDomainValues(SolverVariable variable, SnapshotLabelAssignment assignment, ConstraintNetwork constraintNetwork)
        {
            return variable.GetCandidates();
        }

        private bool IsConsistent(int value, SnapshotLabelAssignment assignment, SolverVariable variable)
        {
            // Has the variable been assigned a value? If it hasn't then the value must be consistent
            if (!assignment.IsAssigned(variable)) return true;

            var labelAssignment = assignment.GetAssignmentFor(variable);

            /*
             * The variable has been assigned a value, it is consistent if the
             * value is the same as the assigned value.
             */
            if (labelAssignment.Value == value) return true;

            return false;
        }

        private SolverVariable SelectUnassignedVariable(ConstraintNetwork constraintNetwork, SnapshotLabelAssignment assignment)
        {
            _variableEnumerator.MoveNext();
            return _variableEnumerator.Current;
        }

        private SolutionSnapshot ConvertSnapshotFrom(SnapshotLabelAssignment assignment, ConstraintNetwork constraintNetwork)
        {
            return new SolutionSnapshot(ExtractSingletonLabelsFrom(assignment, constraintNetwork),
                                        Enumerable.Empty<AggregateVariableLabelModel>());
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

        private IEnumerable<SingletonVariableLabelModel> ExtractSingletonLabelsFrom(ConstraintNetwork constraintNetwork)
        {
            var labelAccumulator = new List<SingletonVariableLabelModel>();
            var allSingletonVariables = constraintNetwork.GetSingletonVariables();

            foreach (var variable in allSingletonVariables)
            {
                var solverVariable = _modelSolverMap.GetSolverSingletonVariableByName(variable.Name);
                var ternaryConstraintExpressionSolutions = _modelSolverMap.FindTernaryConstraintSolutionsInvolving(solverVariable);
                var variableSolverValue = FindValueFrom(solverVariable, ternaryConstraintExpressionSolutions);
                var variableModel = _modelSolverMap.GetModelSingletonVariableByName(variable.Name);
                var variableModelValue = ConvertSolverValueToModel(variableModel, variableSolverValue);
                var label = new SingletonVariableLabelModel(variableModel, new ValueModel(variableModelValue));
                labelAccumulator.Add(label);
            }

            return labelAccumulator;
        }

        private IEnumerable<AggregateVariableLabelModel> ExtractAggregateLabelsFrom(ConstraintNetwork constraintNetwork)
        {
            var aggregatorLabelAccumulator = new List<AggregateVariableLabelModel>();
            var allAggregateVariables = constraintNetwork.GetAggregateVariables();

            foreach (var aggregateSolverVariable in allAggregateVariables)
            {
                var internalAccumulator = new List<ValueModel>();
                var solverVariable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateSolverVariable.Name);
                var aggregateVariableModel = _modelSolverMap.GetModelAggregateVariableByName(aggregateSolverVariable.Name);
                foreach (var variable in solverVariable.Variables)
                {
                    internalAccumulator.Add(new ValueModel(variable.Domain.PossibleValues.First()));
                }

                var label = new AggregateVariableLabelModel(aggregateVariableModel, internalAccumulator);
                aggregatorLabelAccumulator.Add(label);
            }

            return aggregatorLabelAccumulator;
        }

        private int FindValueFrom(SolverVariable solverVariable, IEnumerable<TernaryConstraintExpressionSolution> ternaryConstraintExpressionSolutions)
        {
            var candidateValues = FindCandidateValuesFor(solverVariable, ternaryConstraintExpressionSolutions);
            return candidateValues.First();
        }

        private IEnumerable<int> FindCandidateValuesFor(SolverVariable solverVariable, IEnumerable<TernaryConstraintExpressionSolution> ternaryConstraintExpressionSolutions)
        {
            var allCandidates = solverVariable.GetCandidates();

            if (FindTernaryCandidatesFor(solverVariable, ternaryConstraintExpressionSolutions, out var ternaryCandidates))
            {
                return allCandidates.Intersect(ternaryCandidates);
            }

            return allCandidates;
        }

        private bool FindTernaryCandidatesFor(SolverVariable solverVariable,
                                              IEnumerable<TernaryConstraintExpressionSolution> ternaryConstraintExpressionSolutions,
                                              out IEnumerable<int> ternaryCandidates)
        {
            var candidateAccumulator = new List<int>();

            foreach (var ternaryConstraintExpressionSolution in ternaryConstraintExpressionSolutions)
            {
                var allSets = ternaryConstraintExpressionSolution.DomainValue.GetSets().ToList();

                var leftArcConnector = ternaryConstraintExpressionSolution.Expression.LeftArc.Connector as EncapsulatedVariableConnector;
                Debug.Assert(leftArcConnector != null);
                var leftArcVariableSelector = leftArcConnector.Selector;

                var leftArcLeftVariableNode = leftArcConnector.Left as VariableNode;
                if (leftArcLeftVariableNode != null && leftArcLeftVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        foreach (var set in allSets)
                        {
                            candidateAccumulator.Add(set.GetAt(leftArcVariableSelector.Index - 1));
                        }
                    }
                }

                var leftArcRightVariableNode = leftArcConnector.Right as VariableNode;
                if (leftArcRightVariableNode != null && leftArcRightVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        foreach (var set in allSets)
                        {
                            candidateAccumulator.Add(set.GetAt(leftArcVariableSelector.Index - 1));
                        }
                    }
                }

                var rightArcConnector = ternaryConstraintExpressionSolution.Expression.RightArc.Connector as EncapsulatedVariableConnector;
                Debug.Assert(rightArcConnector != null);
                var rightArcVariableSelector = rightArcConnector.Selector;

                var rightArcLeftVariableNode = rightArcConnector.Left as VariableNode;
                if (rightArcLeftVariableNode != null && rightArcLeftVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        foreach (var set in allSets)
                        {
                            candidateAccumulator.Add(set.GetAt(rightArcVariableSelector.Index - 1));
                        }
                    }
                }

                var rightArcRightVariableNode = rightArcConnector.Right as VariableNode;
                if (rightArcRightVariableNode != null && rightArcRightVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        foreach (var set in allSets)
                        {
                            candidateAccumulator.Add(set.GetAt(rightArcVariableSelector.Index - 1));
                        }
                    }
                }
            }

            ternaryCandidates = candidateAccumulator;
            return ternaryCandidates.Any();
        }

        private object ConvertSolverValueToModel(SingletonVariableModel theVariable, long solverValue)
        {
            var variableDomainValue = _valueMapper.GetDomainValueFor(theVariable);
            return variableDomainValue.MapFrom(solverValue);
        }
    }
}
