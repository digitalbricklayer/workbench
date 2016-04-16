using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Constraint solver implemented using Google or-tools library.
    /// </summary>
    public class OrToolsSolver
    {
        private Google.OrTools.ConstraintSolver.Solver solver;
        private readonly Dictionary<string, Tuple<VariableModel, IntVar>> singletonVariableMap;
        private readonly Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> aggregateVariableMap;
        private ModelModel model;

        /// <summary>
        /// Initialize the constraint solver with default values.
        /// </summary>
        public OrToolsSolver()
        {
            this.singletonVariableMap = new Dictionary<string, Tuple<VariableModel, IntVar>>();
            this.aggregateVariableMap = new Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>>();
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
            var decisionBuilder = solver.MakePhase(variables,
                                                   Google.OrTools.ConstraintSolver.Solver.CHOOSE_FIRST_UNBOUND,
                                                   Google.OrTools.ConstraintSolver.Solver.INT_VALUE_DEFAULT);
            var collector = this.CreateCollector();
            var solveResult = this.solver.Solve(decisionBuilder, collector);
            if (!solveResult) return SolveResult.Failed;

            var theSolutionSnapshot = this.ExtractValuesFrom(collector);
            var solveDuration = TimeSpan.FromMilliseconds(this.solver.WallTime());
            return new SolveResult(SolveStatus.Success, solveDuration, theSolutionSnapshot);
        }

        private void ProcessConstraints(ModelModel theModel)
        {
            foreach (var constraint in theModel.Constraints)
            {
                var expressionConstraint = constraint as ExpressionConstraintModel;
                if (expressionConstraint == null)
                {
                    HandleAllDifferentConstraint((AllDifferentConstraintModel) constraint);
                }
                else
                {
                    HandleExpressionConstraint(expressionConstraint);
                }
            }
        }

        private void HandleAllDifferentConstraint(AllDifferentConstraintModel allDifferentConstraint)
        {
            var x = GetVectorByName(allDifferentConstraint.Variable.Name);
            var orAllDifferentConstraint = this.solver.MakeAllDifferent(x);
            this.solver.Add(orAllDifferentConstraint);
        }

        private void HandleExpressionConstraint(ExpressionConstraintModel expressionConstraint)
        {
            switch (expressionConstraint.Expression.OperatorType)
            {
                case OperatorType.Equals:
                    this.HandleEqualsOperator(expressionConstraint);
                    break;

                case OperatorType.NotEqual:
                    this.HandleNotEqualOperator(expressionConstraint);
                    break;

                case OperatorType.GreaterThanOrEqual:
                    this.HandleGreaterThanOrEqualOperator(expressionConstraint);
                    break;

                case OperatorType.LessThanOrEqual:
                    this.HandleLessThanOrEqualOperator(expressionConstraint);
                    break;

                case OperatorType.Greater:
                    this.HandleGreaterOperator(expressionConstraint);
                    break;

                case OperatorType.Less:
                    this.HandleLessOperator(expressionConstraint);
                    break;

                default:
                    throw new NotImplementedException("Not sure how to represent this operator type.");
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
            {
                var band = this.GetVariableBand(aggregate);
                var orVariableVector = this.solver.MakeIntVarArray(aggregate.AggregateCount,
                                                                   band.Item1,
                                                                   band.Item2,
                                                                   aggregate.Name);
                this.aggregateVariableMap.Add(aggregate.Name,
                                              new Tuple<AggregateVariableModel, IntVarVector>(aggregate, orVariableVector));
                foreach (var orVar in orVariableVector)
                {
                    theVariables.Add(orVar);
                }

            }
        }

        private void ProcessSingletonVariables(ModelModel theModel, IntVarVector variables)
        {
            foreach (var variable in theModel.Singletons)
            {
                var orVariable = this.ProcessVariable(variables, variable);
                this.singletonVariableMap.Add(variable.Name,
                                              new Tuple<VariableModel, IntVar>(variable, orVariable));
            }
        }

#if false
        private void ProcessAggregateVariable(IntVarVector theVariables, AggregateVariableModel aggregate)
        {
            var orVariables = new List<IntVar>();
            foreach (var variable in aggregate.Variables)
            {
                var orVariable = this.ProcessVariable(theVariables, variable);
                orVariables.Add(orVariable);
            }
            this.aggregateVariableMap.Add(aggregate.Name,
                                          new Tuple<AggregateVariableModel, IntVarVector>(aggregate, orVariables));
        }
#endif

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

        private void HandleEqualsOperator(ExpressionConstraintModel constraint)
        {
            Constraint equalsConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                equalsConstraint = this.solver.MakeEquality(lhsVariable, rhsVariable);
            }
            else
            {
                equalsConstraint = this.solver.MakeEquality(lhsVariable,
                                                            constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(equalsConstraint);
        }

        private void HandleNotEqualOperator(ExpressionConstraintModel constraint)
        {
            Constraint notEqualConstraint;
            var lhsExpr = this.GetExpressionFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsExpression)
            {
                var rhsExpr = GetExpressionFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                notEqualConstraint = this.solver.MakeNonEquality(lhsExpr, rhsExpr);
            }
            else if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                notEqualConstraint = this.solver.MakeNonEquality(lhsExpr, rhsVariable);
            }
            else
            {
                notEqualConstraint = this.solver.MakeNonEquality(lhsExpr,
                                                                 constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(notEqualConstraint);
        }

        private void HandleLessOperator(ExpressionConstraintModel constraint)
        {
            Constraint lessConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                lessConstraint = this.solver.MakeLess(lhsVariable, rhsVariable);
            }
            else
            {
                lessConstraint = this.solver.MakeLess(lhsVariable,
                                                      constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(lessConstraint);
        }

        private void HandleGreaterOperator(ExpressionConstraintModel constraint)
        {
            Constraint greaterConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                greaterConstraint = this.solver.MakeGreater(lhsVariable, rhsVariable);
            }
            else
            {
                greaterConstraint = this.solver.MakeGreater(lhsVariable,
                                                            constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(greaterConstraint);
        }

        private void HandleLessThanOrEqualOperator(ExpressionConstraintModel constraint)
        {
            Constraint lessThanOrEqualConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                lessThanOrEqualConstraint = this.solver.MakeLessOrEqual(lhsVariable, rhsVariable);
            }
            else
            {
                lessThanOrEqualConstraint = this.solver.MakeLessOrEqual(lhsVariable,
                                                                        constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(lessThanOrEqualConstraint);
        }

        private void HandleGreaterThanOrEqualOperator(ExpressionConstraintModel constraint)
        {
            Constraint greaterThanOrEqualConstraint;
            var lhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = this.GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable, rhsVariable);
            }
            else
            {
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable, constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(greaterThanOrEqualConstraint);
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

        private IntExpr GetExpressionFrom(ExpressionNode theExpression)
        {
            if (theExpression.IsExpression)
            {
                IntExpr variableExpression;
                VariableExpressionOperatorType op;
                LiteralNode literal;
                if (theExpression.IsSingletonExpression)
                {
                    var singletonExpression = (SingletonVariableReferenceExpressionNode)theExpression.InnerExpression;
                    variableExpression = GetVariableFrom((SingletonVariableReferenceExpressionNode) theExpression.InnerExpression);
                    op = singletonExpression.Operator;
                    literal = singletonExpression.Literal;
                }
                else
                {
                    var aggregateExpression = (SingletonVariableReferenceExpressionNode)theExpression.InnerExpression;
                    variableExpression = GetVariableFrom((AggregateVariableReferenceExpressionNode) theExpression.InnerExpression);
                    op = aggregateExpression.Operator;
                    literal = aggregateExpression.Literal;
                }

                switch (op)
                {
                    case VariableExpressionOperatorType.Add:
                        return this.solver.MakeSum(variableExpression, literal.Value);
                    
                    case VariableExpressionOperatorType.Minus:
                        return this.solver.MakeSum(variableExpression, -literal.Value);

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                return GetVariableFrom(theExpression);
            }
        }

        private IntVar GetVariableFrom(ExpressionNode theExpression)
        {
            Debug.Assert(!theExpression.IsLiteral);

            if (theExpression.IsSingletonReference)
            {
                var singletonVariableReference = (SingletonVariableReferenceNode) theExpression.InnerExpression;
                return this.GetSingletonVariableByName(singletonVariableReference.VariableName);
            }

            var aggregateVariableReference = (AggregateVariableReferenceNode) theExpression.InnerExpression;
            return this.GetAggregateVariableByName(aggregateVariableReference.VariableName,
                                                   aggregateVariableReference.Subscript);
        }

        private IntExpr GetVariableFrom(AggregateVariableReferenceExpressionNode aggregateExpression)
        {
            return GetAggregateVariableByName(aggregateExpression.VariableReference.VariableName,
                                              aggregateExpression.VariableReference.Subscript);
        }

        private IntExpr GetVariableFrom(SingletonVariableReferenceExpressionNode singletonExpression)
        {
            return GetSingletonVariableByName(singletonExpression.VariableReference.VariableName);
        }

        private IntVarVector GetVectorByName(string aggregateName)
        {
            return this.aggregateVariableMap[aggregateName].Item2;
        }
    }
}
