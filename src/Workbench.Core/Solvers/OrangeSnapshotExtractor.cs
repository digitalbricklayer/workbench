using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class OrangeSnapshotExtractor
    {
        private readonly OrangeModelSolverMap _modelSolverMap;
        private readonly ValueMapper _valueMapper;

        internal OrangeSnapshotExtractor(OrangeModelSolverMap modelSolverMap, ValueMapper valueMapper)
        {
            _modelSolverMap = modelSolverMap;
            _valueMapper = valueMapper;
        }

        internal SolutionSnapshot CreateSnapshotFrom(ConstraintNetwork constraintNetwork)
        {
            return new SolutionSnapshot(ExtractSingletonLabelsFrom(constraintNetwork),
                                        ExtractAggregateLabelsFrom(constraintNetwork));
        }

        private IEnumerable<SingletonLabelModel> ExtractSingletonLabelsFrom(ConstraintNetwork constraintNetwork)
        {
            var labelAccumulator = new List<SingletonLabelModel>();
            var allSingletonVariables = constraintNetwork.GetSingletonVariables();

            foreach (var variable in allSingletonVariables)
            {
                var solverVariable = _modelSolverMap.GetSolverSingletonVariableByName(variable.Name);
                var ternaryConstraintExpressionSolutions = _modelSolverMap.FindTernaryConstraintSolutionsInvolving(solverVariable);
                var variableSolverValue = FindValueFrom(solverVariable, ternaryConstraintExpressionSolutions);
                var variableModel = _modelSolverMap.GetModelSingletonVariableByName(variable.Name);
                var variableModelValue = ConvertSolverValueToModel(variableModel, variableSolverValue);
                var label = new SingletonLabelModel(variableModel, new ValueModel(variableModelValue));
                labelAccumulator.Add(label);
            }

            return labelAccumulator;
        }

        private IEnumerable<AggregateLabelModel> ExtractAggregateLabelsFrom(ConstraintNetwork constraintNetwork)
        {
            var aggregatorLabelAccumulator = new List<AggregateLabelModel>();
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

                var label = new AggregateLabelModel(aggregateVariableModel, internalAccumulator);
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
                var allSets = ternaryConstraintExpressionSolution.DomainValue.GetSets();

                var leftArcConnector = ternaryConstraintExpressionSolution.Expression.LeftArc.Connector as EncapsulatedVariableConnector;
                Debug.Assert(leftArcConnector != null);
                var leftArcVariableSelector = leftArcConnector.Selector;

                var leftArcLeftVariableNode = leftArcConnector.Left as VariableNode;
                if (leftArcLeftVariableNode != null && leftArcLeftVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        var set = allSets.First();
                        candidateAccumulator.Add(set.GetAt(leftArcVariableSelector.Index - 1));
                    }
                }

                var leftArcRightVariableNode = leftArcConnector.Right as VariableNode;
                if (leftArcRightVariableNode != null && leftArcRightVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        var set = allSets.First();
                        candidateAccumulator.Add(set.GetAt(leftArcVariableSelector.Index - 1));
                    }
                }

                var rightArcConnector = ternaryConstraintExpressionSolution.Expression.RightArc.Connector as EncapsulatedVariableConnector;
                Debug.Assert(rightArcConnector != null);
                var rightArcVariableSelector = leftArcConnector.Selector;

                var rightArcLeftVariableNode = rightArcConnector.Left as VariableNode;
                if (rightArcLeftVariableNode != null && rightArcLeftVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        var set = allSets.First();
                        candidateAccumulator.Add(set.GetAt(rightArcVariableSelector.Index - 1));
                    }
                }

                var rightArcRightVariableNode = rightArcConnector.Right as VariableNode;
                if (rightArcRightVariableNode != null && rightArcRightVariableNode.Variable.Name == solverVariable.Name)
                {
                    if (!ternaryConstraintExpressionSolution.DomainValue.IsEmpty)
                    {
                        var set = allSets.First();
                        candidateAccumulator.Add(set.GetAt(rightArcVariableSelector.Index - 1));
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
