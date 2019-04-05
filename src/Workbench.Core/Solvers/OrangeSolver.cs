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

            var modelValidator = new ModelValidator(theModel);

            if (!modelValidator.Validate()) return SolveResult.InvalidModel;

            var constraintNetworkBuilder = new ConstraintNetworkBuilder(_modelSolverMap);

            // Create constraint network
            var constraintNetwork = constraintNetworkBuilder.Build(theModel);

            bool domainChanged;

            // Time how long it takes to get a solution
            _stopwatch.Start();

            // Keep revising the constraint network until no domains are altered
            do
            {
                domainChanged = ReviseArcs(constraintNetwork);
            } while (domainChanged);

            _stopwatch.Stop();

			if (!constraintNetwork.IsArcConsistent())
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
                if (!arc.Connector.Constraint.Node.InnerExpression.LeftExpression.IsLiteral)
                {
                    // Revise the left variable domain
                    domainChanged |= ReviseLeft(arc.Left, arc.Right, arc.Connector.Constraint);
                }

                if (!arc.Connector.Constraint.Node.InnerExpression.RightExpression.IsLiteral)
                {
                    // Revise the right variable domain
                    domainChanged |= ReviseRight(arc.Left, arc.Right, arc.Connector.Constraint);
                }
            }

            return domainChanged;
        }

        private bool ReviseLeft(Node leftNode, Node rightNode, ConstraintExpression expression)
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

        private bool ReviseRight(Node leftNode, Node rightNode, ConstraintExpression expression)
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
}
