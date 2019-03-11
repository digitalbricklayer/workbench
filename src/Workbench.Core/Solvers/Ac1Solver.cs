using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Implementation of the solver using the AC-1 algorithm.
    /// </summary>
    public class Ac1Solver : ISolvable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Ac1Cache _cache = new Ac1Cache();

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
            var constraintNetwork = new ConstraintNetworkBuilder(_cache).Build(theModel);

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
                // Revise the left variable domain
                domainChanged |= ReviseLeft(arc.Left, arc.Right, arc.Constraint);
                // Revise the right variable domain
                domainChanged |= ReviseRight(arc.Left, arc.Right, arc.Constraint);
            }

            return domainChanged;
        }

        private bool ReviseLeft(IntegerVariable leftVariable, IntegerVariable rightVariable, ConstraintExpression expression)
        {
            var expressionEvaluator = new ValueEvaluator(leftVariable, rightVariable, expression);
            var isDomainChanged = false;
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in leftVariable.Domain.PossibleValues)
            {
                if (!expressionEvaluator.EvaluateLeft(possibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                    isDomainChanged = true;
                }
            }

            foreach (var valueToRemove in valuesToRemove)
            {
                leftVariable.Domain.Remove(valueToRemove);
            }

            return isDomainChanged;
        }

        private bool ReviseRight(IntegerVariable leftVariable, IntegerVariable rightVariable, ConstraintExpression expression)
        {
            var expressionEvaluator = new ValueEvaluator(leftVariable, rightVariable, expression);
            var isDomainChanged = false;
            var valuesToRemove = new List<int>();
            foreach (var possibleValue in rightVariable.Domain.PossibleValues)
            {
                if (!expressionEvaluator.EvaluateRight(possibleValue))
                {
                    valuesToRemove.Add(possibleValue);
                    isDomainChanged = true;
                }
            }

            foreach (var valueToRemove in valuesToRemove)
            {
                rightVariable.Domain.Remove(valueToRemove);
            }

            return isDomainChanged;
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
                var variableModel = _cache.GetModelSingletonVariableByName(variable.Name);
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
                var solverVariable = _cache.GetSolverAggregateVariableByName(aggregateVariable.Name);
                var modelVariable = _cache.GetModelAggregateVariableByName(aggregateVariable.Name);
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
