using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Implementation of the orange solver.
    /// </summary>
    public sealed class OrangeSolver : ISolvable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly OrangeSnapshotExtractor _snapshotExtractor;
        private readonly OrangeModelSolverMap _modelSolverMap;
        private readonly ValueMapper _valueMapper = new ValueMapper();

        /// <summary>
        /// Initialize an orange solver with default values.
        /// </summary>
        public OrangeSolver()
        {
            _modelSolverMap = new OrangeModelSolverMap();
            _snapshotExtractor = new OrangeSnapshotExtractor(_modelSolverMap, _valueMapper);
        }

        /// <summary>
        /// Solve the model using the AC-1 algorithm.
        /// </summary>
        /// <param name="theModel">The model to solve.</param>
        /// <returns>Solve result.</returns>
        public SolveResult Solve(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            var modelValidator = new ModelValidator(theModel);

            if (!modelValidator.Validate()) return SolveResult.InvalidModel;

            var constraintNetworkBuilder = new ConstraintNetworkBuilder(_modelSolverMap, _valueMapper);

            // Create constraint network
            var constraintNetwork = constraintNetworkBuilder.Build(theModel);

            // Time how long it takes to get a solution
            _stopwatch.Start();

            bool domainChanged;

            // Keep revising the constraint network until no domains are altered
            do
            {
                domainChanged = ReviseArcs(constraintNetwork);
            } while (domainChanged);

            _stopwatch.Stop();

            RemoveTernaryValuesInconsistentWithBinaryConstraints(constraintNetwork);

			if (!constraintNetwork.IsArcConsistent())
			{
				return SolveResult.Failed;
			}
			
            // Bind the results to variables labels
            var snapshotStatus = _snapshotExtractor.ExtractFrom(constraintNetwork, out var solutionSnapshot);

            return snapshotStatus ? new SolveResult(SolveStatus.Success, solutionSnapshot, _stopwatch.Elapsed) : SolveResult.Failed;
        }

        private bool ReviseArcs(ConstraintNetwork constraintNetwork)
        {
            var domainChanged = false;

            foreach (var arc in constraintNetwork.GetArcs())
            {
                // Arcs with ternary or unary expressions are pre-computed with one
                // or more solution sets produced for the variables involved in the expression.
                if (arc.IsPreComputed) continue;

                var connector = arc.Connector as ConstraintExpressionConnector;
                var constraint = connector.Constraint;
                if (!constraint.Node.InnerExpression.LeftExpression.IsLiteral)
                {
                    // Revise the left variable domain
                    domainChanged |= ReviseLeft(arc.Left as VariableNode, arc.Right as VariableNode, constraint);
                }

                if (!constraint.Node.InnerExpression.RightExpression.IsLiteral)
                {
                    // Revise the right variable domain
                    domainChanged |= ReviseRight(arc.Left as VariableNode, arc.Right as VariableNode, constraint);
                }
            }

            return domainChanged;
        }

        private bool ReviseLeft(VariableNode leftNode, VariableNode rightNode, BinaryConstraintExpression expression)
        {
            var leftDomainRange = leftNode.Variable.Domain;
            IReadOnlyCollection<int> rightPossibleValues;
            if (!expression.Node.InnerExpression.RightExpression.IsLiteral)
            {
                rightPossibleValues = rightNode.Variable.Domain.PossibleValues;
            }
            else
            {
                var rightLiteral = expression.Node.InnerExpression.RightExpression.GetLiteral();
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

        private bool ReviseRight(VariableNode leftNode, VariableNode rightNode, BinaryConstraintExpression expression)
        {
            var leftDomainRange = leftNode.Variable.Domain;
            var rightDomainRange = rightNode.Variable.Domain;
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

        /// <summary>
        /// Remove values found during the ternary pre-calculation phase that are now
        /// inconsistent with values discovered during the regular revising of
        /// domains of the binary constraints.
        /// </summary>
        /// <param name="constraintNetwork">Constraint network.</param>
        private void RemoveTernaryValuesInconsistentWithBinaryConstraints(ConstraintNetwork constraintNetwork)
        {
            var constraintExpressionSolutions = _modelSolverMap.GetTernaryConstraintExpressionSolutions();

            foreach (var constraintExpressionSolution in constraintExpressionSolutions)
            {
                var leftVariable = constraintExpressionSolution.Expression.LeftArc.Left.Content as SolverVariable;
                Debug.Assert(leftVariable != null);
                var leftVariableDomain = leftVariable.Domain;
                var leftVariableIndex = 1;      // Left variable is always in first index

                RemoveTernaryValuesInconsistentWithBinaryConstraint(constraintExpressionSolution, leftVariableIndex, leftVariableDomain);

                var rightVariable = constraintExpressionSolution.Expression.RightArc.Left.Content as SolverVariable;
                Debug.Assert(rightVariable != null);
                var rightVariableDomain = rightVariable.Domain;
                var rightVariableIndex = 2;      // Right variable is always in second index

                RemoveTernaryValuesInconsistentWithBinaryConstraint(constraintExpressionSolution, rightVariableIndex, rightVariableDomain);
            }
        }

        private static void RemoveTernaryValuesInconsistentWithBinaryConstraint(TernaryConstraintExpressionSolution constraintExpressionSolution,
                                                                                int variableIndex,
                                                                                DomainRange variableDomain)
        {
            var valueSetsToRemove = new List<ValueSet>();

            foreach (var valueSet in constraintExpressionSolution.DomainValue.GetSets())
            {
                // Make sure the set does not violate the domain values
                var variableValue = valueSet.GetAt(variableIndex - 1);

                if (variableDomain.PossibleValues.All(value => value != variableValue))
                {
                    valueSetsToRemove.Add(valueSet);
                }
            }

            if (valueSetsToRemove.Any())
            {
                constraintExpressionSolution.DomainValue.RemoveSets(valueSetsToRemove);
            }
        }
    }
}
