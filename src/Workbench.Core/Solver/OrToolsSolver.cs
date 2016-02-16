using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Constraint solver implemented using Google or-tools library.
    /// </summary>
    public class OrToolsSolver
    {
        private Google.OrTools.ConstraintSolver.Solver solver;
        private readonly Dictionary<string, Tuple<VariableModel, IntVar>> singletonVariableMap;
        private readonly Dictionary<string, Tuple<AggregateVariableModel, List<IntVar>>> aggregateVariableMap;
        private ModelModel model;

        /// <summary>
        /// Initialize the constraint solver with default values.
        /// </summary>
        public OrToolsSolver()
        {
            this.singletonVariableMap = new Dictionary<string, Tuple<VariableModel, IntVar>>();
            this.aggregateVariableMap = new Dictionary<string, Tuple<AggregateVariableModel, List<IntVar>>>();
        }

        /// <summary>
        /// Solve the problem in the model.
        /// </summary>
        /// <param name="theModel">The model to solve.</param>
        public SolveResult Solve(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            this.model = theModel;

            if (!theModel.Validate()) return SolveResult.InvalidModel;

            this.solver = new Google.OrTools.ConstraintSolver.Solver(theModel.Name);

            var variables = this.ProcessVariables(theModel);
            this.ProcessConstraints(theModel);

            // Search
            var db = solver.MakePhase(variables,
                                      Google.OrTools.ConstraintSolver.Solver.CHOOSE_FIRST_UNBOUND,
                                      Google.OrTools.ConstraintSolver.Solver.INT_VALUE_DEFAULT);
            var collector = this.CreateCollector();
            var solveResult = this.solver.Solve(db, collector);
            if (!solveResult) return SolveResult.Failed;

            var theSolutionSnapshot = this.ExtractValuesFrom(collector);
            return new SolveResult(SolveStatus.Success, theSolutionSnapshot);
        }

        private void ProcessConstraints(ModelModel theModel)
        {
            foreach (var constraint in theModel.Constraints)
            {
                switch (constraint.Expression.OperatorType)
                {
                    case OperatorType.Equals:
                        this.HandleEqualsOperator(constraint);
                        break;

                    case OperatorType.GreaterThanOrEqual:
                        this.HandleGreaterThanOrEqualOperator(constraint);
                        break;

                    case OperatorType.LessThanOrEqual:
                        this.HandleLessThanOrEqualOperator(constraint);
                        break;

                    case OperatorType.NotEqual:
                        this.HandleNotEqualOperator(constraint);
                        break;

                    case OperatorType.Greater:
                        this.HandleGreaterOperator(constraint);
                        break;

                    case OperatorType.Less:
                        this.HandleLessOperator(constraint);
                        break;

                    default:
                        throw new NotImplementedException("Not sure how to represent this operator type.");
                }
            }
        }

        private IntVarVector ProcessVariables(ModelModel theModel)
        {
            var variables = new IntVarVector();
            this.ProcessSingletonVariables(theModel, variables);
            this.ProcessAggregateVariables(theModel, variables);
            
            return variables;
        }

        private void ProcessAggregateVariables(ModelModel theModel, IntVarVector theVariables)
        {
            foreach (var aggregate in theModel.Aggregates)
                this.ProcessAggregateVariable(theVariables, aggregate);
        }

        private void ProcessSingletonVariables(ModelModel theModel, IntVarVector variables)
        {
            foreach (var variable in theModel.Variables)
            {
                var orVariable = this.ProcessVariable(variables, variable);
                this.singletonVariableMap.Add(variable.Name,
                                              new Tuple<VariableModel, IntVar>(variable, orVariable));
            }
        }

        private void ProcessAggregateVariable(IntVarVector theVariables, AggregateVariableModel aggregate)
        {
            var orVariables = new List<IntVar>();
            foreach (var variable in aggregate.Variables)
            {
                var orVariable = this.ProcessVariable(theVariables, variable);
                orVariables.Add(orVariable);
            }
            this.aggregateVariableMap.Add(aggregate.Name,
                                          new Tuple<AggregateVariableModel, List<IntVar>>(aggregate, orVariables));
        }

        private IntVar ProcessVariable(IntVarVector variables, VariableModel variable)
        {
            var band = this.GetVariableBand(variable);
            var orVariable = solver.MakeIntVar(band.Item1, band.Item2, variable.Name);
            variables.Add(orVariable);

            return orVariable;
        }

        private Tuple<long,long> GetVariableBand(VariableModel theVariable)
        {
            Debug.Assert(!theVariable.DomainExpression.IsEmpty);

            if (theVariable.DomainExpression.InlineDomain != null)
            {
                var inlineDomain = theVariable.DomainExpression.InlineDomain;
                return new Tuple<long, long>(inlineDomain.LowerBand,
                                             inlineDomain.UpperBand);
            }

            var sharedDomainName = theVariable.DomainExpression.DomainReference.DomainName;
            var sharedDomain = this.model.GetSharedDomainByName(sharedDomainName);
            return new Tuple<long, long>(sharedDomain.Expression.LowerBand,
                                         sharedDomain.Expression.UpperBand);
        }

        private void HandleLessOperator(ConstraintModel constraint)
        {
            Constraint lessConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Left);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Right);
                lessConstraint = this.solver.MakeLess(lhsVariable, rhsVariable);
            }
            else
            {
                lessConstraint = this.solver.MakeLess(lhsVariable,
                                                    constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(lessConstraint);
        }

        private void HandleGreaterOperator(ConstraintModel constraint)
        {
            Constraint greaterConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Left);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Right);
                greaterConstraint = this.solver.MakeGreater(lhsVariable, rhsVariable);
            }
            else
            {
                greaterConstraint = this.solver.MakeGreater(lhsVariable,
                                                       constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(greaterConstraint);
        }

        private void HandleNotEqualOperator(ConstraintModel constraint)
        {
            Constraint notEqualConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Left);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Right);
                notEqualConstraint = this.solver.MakeNonEquality(lhsVariable, rhsVariable);
            }
            else
            {
                notEqualConstraint = this.solver.MakeNonEquality(lhsVariable,
                                                           constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(notEqualConstraint);
        }

        private void HandleLessThanOrEqualOperator(ConstraintModel constraint)
        {
            Constraint lessThanOrEqualConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Left);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Right);
                lessThanOrEqualConstraint = this.solver.MakeLessOrEqual(lhsVariable, rhsVariable);
            }
            else
            {
                lessThanOrEqualConstraint = this.solver.MakeLessOrEqual(lhsVariable,
                    constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(lessThanOrEqualConstraint);
        }

        private void HandleGreaterThanOrEqualOperator(ConstraintModel constraint)
        {
            Constraint greaterThanOrEqualConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Left);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Right);
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable, rhsVariable);
            }
            else
            {
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable,
                                                                              constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(greaterThanOrEqualConstraint);
        }

        private void HandleEqualsOperator(ConstraintModel constraint)
        {
            Constraint equalsConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Left);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Right);
                equalsConstraint = this.solver.MakeEquality(lhsVariable, rhsVariable);
            }
            else
            {
                equalsConstraint = this.solver.MakeEquality(lhsVariable,
                                                        constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(equalsConstraint);
        }

        private SolutionCollector CreateCollector()
        {
            var collector = this.solver.MakeFirstSolutionCollector();
            foreach (var variableTuple in this.singletonVariableMap)
                collector.Add(variableTuple.Value.Item2);
            foreach (var variableTuple in this.aggregateVariableMap)
            {
                var variablesInsideAggregate = variableTuple.Value.Item2;
                foreach (var intVar in variablesInsideAggregate)
                    collector.Add(intVar);
            }

            return collector;
        }

        private SolutionSnapshot ExtractValuesFrom(SolutionCollector solutionCollector)
        {
            var theSnapshot = new SolutionSnapshot();
            foreach (var variableTuple in this.singletonVariableMap)
            {
                var boundValue = solutionCollector.Value(0, variableTuple.Value.Item2);
                var newValue = new ValueModel(variableTuple.Value.Item1, Convert.ToInt32(boundValue));
                theSnapshot.AddSingletonValue(newValue);
            }

            foreach (var aggregateTuple in this.aggregateVariableMap)
            {
                var newValues = new List<int>();
                var orVariables = aggregateTuple.Value.Item2;
                foreach (var orVariable in orVariables)
                {
                    var boundValue = solutionCollector.Value(0, orVariable);
                    newValues.Add(Convert.ToInt32(boundValue));
                }
                var newValue = new ValueModel(aggregateTuple.Value.Item1, newValues);
                theSnapshot.AddAggregateValue(newValue);
            }

            return theSnapshot;
        }

        private IntVar GetSingletonVariableByName(string theVariableName)
        {
            return this.singletonVariableMap[theVariableName].Item2;
        }

        private IntVar GetAggregateVariableByName(string theVariableName, int index)
        {
            var orVariables = this.aggregateVariableMap[theVariableName].Item2;
            return orVariables[index-1];
        }

        private IntVar GetVariableFrom(Expression theExpression)
        {
            Debug.Assert(!theExpression.IsLiteral);

            if (theExpression.IsSingleton)
                return this.GetSingletonVariableByName(theExpression.Variable.Name);

            return this.GetAggregateVariableByName(theExpression.AggregateReference.IdentifierName,
                                                   theExpression.AggregateReference.Index);
        }
    }
}
