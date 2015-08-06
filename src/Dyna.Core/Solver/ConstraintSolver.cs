using System;
using System.Collections.Generic;
using Dyna.Core.Entities;
using Google.OrTools.ConstraintSolver;
using Constraint = Dyna.Core.Entities.Constraint;

namespace Dyna.Core.Solver
{
    /// <summary>
    /// Constraint solver.
    /// </summary>
    public class ConstraintSolver
    {
        private Google.OrTools.ConstraintSolver.Solver solver;
        private readonly Dictionary<string, Tuple<Variable, IntVar>> variableMap = new Dictionary<string, Tuple<Variable, IntVar>>();

        /// <summary>
        /// Solve the problem in the workspace.
        /// </summary>
        /// <param name="theModel">The problem workspace.</param>
        public SolveResult Solve(Model theModel)
        {
            if (theModel == null)
                throw new ArgumentNullException("theModel");

            if (!theModel.Validate()) return SolveResult.InvalidModel;

            this.solver = new Google.OrTools.ConstraintSolver.Solver(theModel.Name);

            // domains
            var variables = new IntVarVector();
            foreach (var variable in theModel.Variables)
            {
                var orVariable = solver.MakeIntVar(variable.Domain.Expression.LowerBand,
                                                   variable.Domain.Expression.UpperBand,
                                                   variable.Name);
                variables.Add(orVariable);
                this.variableMap.Add(variable.Name,
                                     new Tuple<Variable, IntVar>(variable, orVariable));
            }

            // Variables
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

            // Search
            var db = solver.MakePhase(variables,
                                      Google.OrTools.ConstraintSolver.Solver.CHOOSE_FIRST_UNBOUND,
                                      Google.OrTools.ConstraintSolver.Solver.INT_VALUE_DEFAULT);
            var collector = this.CreateCollector();
            var solveResult = this.solver.Solve(db, collector);
            if (!solveResult) return SolveResult.Failed;

            var boundVariables = this.CreateBoundVariablesFrom(collector);
            var theSolution = new Solution(theModel, boundVariables);
            return new SolveResult(SolveStatus.Success, theSolution);
        }

        private void HandleLessOperator(Constraint constraint)
        {
            Google.OrTools.ConstraintSolver.Constraint lessConstraint;
            var lhsVariable = this.GetVariableByName(constraint.Expression.Left.Name);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableByName(constraint.Expression.Right.Variable.Name);
                lessConstraint = this.solver.MakeLess(lhsVariable, rhsVariable);
            }
            else
            {
                lessConstraint = this.solver.MakeLess(lhsVariable,
                                                    constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(lessConstraint);
        }

        private void HandleGreaterOperator(Constraint constraint)
        {
            Google.OrTools.ConstraintSolver.Constraint greaterConstraint;
            var lhsVariable = this.GetVariableByName(constraint.Expression.Left.Name);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableByName(constraint.Expression.Right.Variable.Name);
                greaterConstraint = this.solver.MakeGreater(lhsVariable, rhsVariable);
            }
            else
            {
                greaterConstraint = this.solver.MakeGreater(lhsVariable,
                                                       constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(greaterConstraint);
        }

        private void HandleNotEqualOperator(Constraint constraint)
        {
            Google.OrTools.ConstraintSolver.Constraint notEqualConstraint;
            var lhsVariable = this.GetVariableByName(constraint.Expression.Left.Name);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableByName(constraint.Expression.Right.Variable.Name);
                notEqualConstraint = this.solver.MakeNonEquality(lhsVariable, rhsVariable);
            }
            else
            {
                notEqualConstraint = this.solver.MakeNonEquality(lhsVariable,
                                                           constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(notEqualConstraint);
        }

        private void HandleLessThanOrEqualOperator(Constraint constraint)
        {
            {
                Google.OrTools.ConstraintSolver.Constraint lessThanOrEqualConstraint;
                var lhsVariable = this.GetVariableByName(constraint.Expression.Left.Name);
                if (constraint.Expression.Right.IsVarable)
                {
                    var rhsVariable = this.GetVariableByName(constraint.Expression.Right.Variable.Name);
                    lessThanOrEqualConstraint = this.solver.MakeLessOrEqual(lhsVariable, rhsVariable);
                }
                else
                {
                    lessThanOrEqualConstraint = this.solver.MakeLessOrEqual(lhsVariable,
                                                               constraint.Expression.Right.Literal.Value);
                }
                this.solver.Add(lessThanOrEqualConstraint);
            }
        }

        private void HandleGreaterThanOrEqualOperator(Constraint constraint)
        {
            Google.OrTools.ConstraintSolver.Constraint greaterThanOrEqualConstraint;
            var lhsVariable = this.GetVariableByName(constraint.Expression.Left.Name);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableByName(constraint.Expression.Right.Variable.Name);
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable, rhsVariable);
            }
            else
            {
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable,
                                                              constraint.Expression.Right.Literal.Value);
            }
            this.solver.Add(greaterThanOrEqualConstraint);
        }

        private void HandleEqualsOperator(Constraint constraint)
        {
            Google.OrTools.ConstraintSolver.Constraint equalsConstraint;
            var lhsVariable = this.GetVariableByName(constraint.Expression.Left.Name);
            if (constraint.Expression.Right.IsVarable)
            {
                var rhsVariable = this.GetVariableByName(constraint.Expression.Right.Variable.Name);
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
            foreach (var variableTuple in this.variableMap)
                collector.Add(variableTuple.Value.Item2);

            return collector;
        }

        private BoundVariable[] CreateBoundVariablesFrom(SolutionCollector solutionCollector)
        {
            var boundVariables = new List<BoundVariable>();
            foreach (var variableTuple in this.variableMap)
            {
                var boundVariable = new BoundVariable(variableTuple.Value.Item1);
                var boundValue = solutionCollector.Value(0, variableTuple.Value.Item2);
                boundVariable.Value = Convert.ToInt32(boundValue);
                boundVariables.Add(boundVariable);
            }

            return boundVariables.ToArray();
        }

        private IntVar GetVariableByName(string theVariableName)
        {
            return this.variableMap[theVariableName].Item2;
        }
    }
}
