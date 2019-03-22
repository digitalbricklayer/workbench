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
                if (arc.A is VariableNode && arc.B is VariableNode)
                {
                    var leftVariableNode = (VariableNode) arc.A;
                    var rightVariableNode = (VariableNode) arc.B;

                    // Revise the left variable domain
                    domainChanged |= ReviseLeft(leftVariableNode.Variable, rightVariableNode.Variable, arc.Constraint);
                    // Revise the right variable domain
                    domainChanged |= ReviseRight(leftVariableNode.Variable, rightVariableNode.Variable, arc.Constraint);
                }
                else if (arc.A is VariableNode && arc.B is LiteralNode)
                {
                    var leftVariableNode = (VariableNode) arc.A;
                    var literalNode = (LiteralNode) arc.B;

                    // Revise the left variable domain, the right is a literal
                    domainChanged |= Revise(leftVariableNode.Variable, literalNode, arc.Constraint);
                }
            }

            return domainChanged;
        }

        private bool ReviseLeft(IntegerVariable leftVariable, IntegerVariable rightVariable, ConstraintExpression expression)
        {
            var expressionEvaluator = new ValueEvaluator(leftVariable.Domain.PossibleValues, rightVariable.Domain.PossibleValues, expression);
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in leftVariable.Domain.PossibleValues)
            {
                if (!expressionEvaluator.EvaluateLeft(possibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                }
            }

            valuesToRemove.ForEach(valueToRemove => leftVariable.Domain.Remove(valueToRemove));

            return valuesToRemove.Any();
        }

        private bool ReviseRight(IntegerVariable leftVariable, IntegerVariable rightVariable, ConstraintExpression expression)
        {
            var expressionEvaluator = new ValueEvaluator(leftVariable.Domain.PossibleValues, rightVariable.Domain.PossibleValues, expression);
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in rightVariable.Domain.PossibleValues)
            {
                if (!expressionEvaluator.EvaluateRight(possibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                }
            }

            valuesToRemove.ForEach(valueToRemove => rightVariable.Domain.Remove(valueToRemove));

            return valuesToRemove.Any();
        }

        private bool Revise(IntegerVariable leftVariable, LiteralNode rightLiteral, ConstraintExpression expression)
        {
            var literalValue = new ReadOnlyCollection<int>(new List<int>{rightLiteral.Literal});
            var expressionEvaluator = new ValueEvaluator(leftVariable.Domain.PossibleValues, literalValue, expression);
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in leftVariable.Domain.PossibleValues)
            {
                if (!expressionEvaluator.EvaluateLeft(possibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                }
            }

            valuesToRemove.ForEach(valueToRemove => leftVariable.Domain.Remove(valueToRemove));

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
