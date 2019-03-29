using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Implementation of the orange solver.
    /// </summary>
    public class OrangeSolver : ISolvable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly OrangeModelSolverMap _modelSolverMap = new OrangeModelSolverMap();
        private readonly PossibleValueExtractor _domainExtractor;

        /// <summary>
        /// Initialize an orange solver with default values.
        /// </summary>
        public OrangeSolver()
        {
            _domainExtractor = new PossibleValueExtractor(_modelSolverMap);
        }

        /// <summary>
        /// Solve the model using the AC-1 algorithm.
        /// </summary>
        /// <param name="theModel">The model to solve.</param>
        /// <returns>Solve result.</returns>
        public SolveResult Solve(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            if (!new ModelValidator(theModel).Validate()) return SolveResult.InvalidModel;

            // Create constraint network
            var constraintNetwork = new ConstraintNetworkBuilder(_modelSolverMap).Build(theModel);

            bool domainChanged;

            // Time how long it takes to get a solution
            _stopwatch.Start();

            // Keep revising the constraint network until no domains are altered
            do
            {
                domainChanged = ReviseArcs(constraintNetwork);
            } while (domainChanged);

            _stopwatch.Stop();

			if (!constraintNetwork.IsSolved)
			{
				return SolveResult.Failed;
			}
			
            // Bind the variables to labels
			return CreateSolveResult(constraintNetwork, _stopwatch.Elapsed);
        }

        private bool ReviseArcs(ConstraintNetwork constraintNetwork)
        {
            var domainChanged = false;

            foreach (var arc in constraintNetwork.ToArray())
            {
                if (arc.A.IsRevisable())
                {
                    // Revise the left variable domain
                    domainChanged |= ReviseLeft(arc.A, arc.B, arc.Constraint);
                }

                if (arc.B.IsRevisable())
                {
                    // Revise the right variable domain
                    domainChanged |= ReviseRight(arc.A, arc.B, arc.Constraint);
                }
            }

            return domainChanged;
        }

        private bool ReviseLeft(Node leftNode, Node rightNode, ConstraintExpression expression)
        {
            var leftDomainRange = _domainExtractor.ExtractFrom(leftNode);
            IReadOnlyCollection<int> rightPossibleValues;
            if (rightNode.IsRevisable())
            {
                rightPossibleValues = _domainExtractor.ExtractFrom(rightNode).PossibleValues;
            }
            else
            {
                var rightLiteral = rightNode.Expression.GetLiteral();
                rightPossibleValues = new ReadOnlyCollection<int>(new List<int> { rightLiteral });
            }
            var valueEvaluator = new LeftValueEvaluator(rightPossibleValues, expression);
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in leftDomainRange.PossibleValues)
            {
                var valueAdjuster = new ExpressionEvaluator(_modelSolverMap);
                var evaluatedPossibleValue = valueAdjuster.Evaluate(expression.Node.InnerExpression.LeftExpression, possibleValue);
                if (!valueEvaluator.EvaluateLeft(evaluatedPossibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                }
            }

            leftDomainRange.RemoveAll(valuesToRemove);

            return valuesToRemove.Any();
        }

        private bool ReviseRight(Node leftNode, Node rightNode, ConstraintExpression expression)
        {
            var leftDomainRange = _domainExtractor.ExtractFrom(leftNode);
            var rightDomainRange = _domainExtractor.ExtractFrom(rightNode);
            var valueEvaluator = new RightValueEvaluator(leftDomainRange.PossibleValues, expression);
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in rightDomainRange.PossibleValues)
            {
                var valueAdjuster = new ExpressionEvaluator(_modelSolverMap);
                var evaluatedPossibleValue = valueAdjuster.Evaluate(expression.Node.InnerExpression.RightExpression, possibleValue);
                if (!valueEvaluator.EvaluateRight(evaluatedPossibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                }
            }

            rightDomainRange.RemoveAll(valuesToRemove);

            return valuesToRemove.Any();
        }

        private SolveResult CreateSolveResult(ConstraintNetwork constraintNetwork, TimeSpan elapsedTime)
		{
            return new SolveResult(SolveStatus.Success, CreateSnapshotFrom(constraintNetwork, elapsedTime));
		}

        private SolutionSnapshot CreateSnapshotFrom(ConstraintNetwork constraintNetwork, TimeSpan elapsedTime)
        {
            return new SolutionSnapshot(ExtractSingletonLabelsFrom(constraintNetwork),
                                        ExtractAggregateLabelsFrom(constraintNetwork),
                                        elapsedTime);
        }

        private IEnumerable<SingletonLabelModel> ExtractSingletonLabelsFrom(ConstraintNetwork constraintNetwork)
        {
            var labelAccumulator = new List<SingletonLabelModel>();
            var allVariables = constraintNetwork.GetSingletonVariables();

            foreach (var variable in allVariables)
            {
                var variableModel = _modelSolverMap.GetModelSingletonVariableByName(variable.Name);
                var label = new SingletonLabelModel(variableModel, new ValueModel(variable.Domain.PossibleValues.First()));
                labelAccumulator.Add(label);
            }

            return labelAccumulator;
        }

        private IEnumerable<CompoundLabelModel> ExtractAggregateLabelsFrom(ConstraintNetwork constraintNetwork)
        {
            var compoundLabelAccumulator = new List<CompoundLabelModel>();
            var allAggregateVariables = constraintNetwork.GetAggregateVariables();

            foreach (var aggregateVariable in allAggregateVariables)
            {
                var internalAccumulator = new List<ValueModel>();
                var solverVariable = _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariable.Name);
                var modelVariable = _modelSolverMap.GetModelAggregateVariableByName(aggregateVariable.Name);
                foreach (var variable in solverVariable.Variables)
                {
                    internalAccumulator.Add(new ValueModel(variable.Domain.PossibleValues.First()));
                }

                var label = new CompoundLabelModel(modelVariable, internalAccumulator);
                compoundLabelAccumulator.Add(label);
            }

            return compoundLabelAccumulator;
        }
    }

    internal sealed class PossibleValueExtractor
    {
        private readonly OrangeModelSolverMap _map;

        internal PossibleValueExtractor(OrangeModelSolverMap map)
        {
            Contract.Requires<ArgumentNullException>(map != null);
            _map = map;
        }

        internal DomainRange ExtractFrom(Node node)
        {
            switch (node.Expression.InnerExpression)
            {
                case SingletonVariableReferenceExpressionNode singletonVariableReferenceExpressionNode:
                    var x = _map.GetSolverSingletonVariableByName(singletonVariableReferenceExpressionNode.VariableReference.VariableName);
                    return x.Domain;

                case AggregateVariableReferenceExpressionNode aggregateVariableReferenceExpressionNode:
                    return GetRangeFrom(aggregateVariableReferenceExpressionNode);

                case SingletonVariableReferenceNode singletonVariableReferenceNode:
                    var singletonSolverVariable = _map.GetSolverSingletonVariableByName(singletonVariableReferenceNode.VariableName);
                    return singletonSolverVariable.Domain;

                case AggregateVariableReferenceNode aggregateVariableReferenceNode:
                    return GetRangeFrom(aggregateVariableReferenceNode);

                default:
                    throw new NotImplementedException();
            }
        }

        private DomainRange GetRangeFrom(AggregateVariableReferenceNode aggregateVariableReferenceNode)
        {
            var aggregateSolverVariable = _map.GetSolverAggregateVariableByName(aggregateVariableReferenceNode.VariableName);
            var subscriptVariable = aggregateSolverVariable.GetAt(aggregateVariableReferenceNode.SubscriptStatement.Subscript);
            return subscriptVariable.Domain;
        }

        private DomainRange GetRangeFrom(AggregateVariableReferenceExpressionNode aggregateVariableReferenceExpressionNode)
        {
            var subscriptVariable = _map.GetSolverAggregateVariableByName(aggregateVariableReferenceExpressionNode.VariableReference.VariableName,
                                                                          aggregateVariableReferenceExpressionNode.VariableReference.SubscriptStatement.Subscript);
            return subscriptVariable.Domain;
        }
    }

    /// <summary>
    /// Evaluate one side of a constraint expressions.
    /// </summary>
    internal sealed class ExpressionEvaluator
    {
        private readonly OrangeModelSolverMap _map;

        internal ExpressionEvaluator(OrangeModelSolverMap map)
        {
            Contract.Requires<ArgumentNullException>(map != null);
            _map = map;
        }

        internal int Evaluate(ExpressionNode expression, int possibleValue)
        {
            if (!expression.IsExpression) return possibleValue;

            VariableExpressionOperatorType op;
            InfixStatementNode infixStatement;
            if (expression.IsSingletonExpression)
            {
                var singletonExpression = (SingletonVariableReferenceExpressionNode) expression.InnerExpression;
                op = singletonExpression.Operator;
                infixStatement = singletonExpression.InfixStatement;
            }
            else
            {
                var aggregateExpression = (AggregateVariableReferenceExpressionNode) expression.InnerExpression;
                op = aggregateExpression.Operator;
                infixStatement = aggregateExpression.InfixStatement;
            }

            switch (op)
            {
                case VariableExpressionOperatorType.Add:
                    return possibleValue + GetValueFrom(infixStatement);

                case VariableExpressionOperatorType.Subtract:
                    return possibleValue - GetValueFrom(infixStatement);

                default:
                    throw new NotImplementedException();
            }
        }

        private int GetValueFrom(InfixStatementNode infixStatement)
        {
            Contract.Requires<ArgumentNullException>(infixStatement != null);
            Contract.Assume(infixStatement.IsLiteral);

            return infixStatement.Literal.Value;
        }
    }
}
