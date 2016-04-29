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
    /// Convert the model representation into a representation usable 
    /// with the or-tools solver.
    /// </summary>
    class ModelConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly Dictionary<string, Tuple<VariableModel, IntVar>> singletonVariableMap;
        private readonly Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> aggregateVariableMap;
        private ModelModel model;
        private IntVarVector variables;

        /// <summary>
        /// Initialize the model converter with a Google or-tools solver.
        /// </summary>
        public ModelConverter(Google.OrTools.ConstraintSolver.Solver theSolver)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            this.singletonVariableMap = new Dictionary<string, Tuple<VariableModel, IntVar>>();
            this.aggregateVariableMap = new Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>>();
            this.solver = theSolver;
        }

        public Dictionary<string, Tuple<VariableModel, IntVar>> SingletonVariableMap => this.singletonVariableMap;
        public Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> AggregateVariableMap => this.aggregateVariableMap;
        public IntVarVector Variables => this.variables;

        /// <summary>
        /// Convert the model into a representation used by the Google or-tools solver.
        /// </summary>
        /// <param name="theModel">The model model.</param>
        public void ConvertFrom(ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.model = theModel;
            this.variables = ProcessVariables(theModel);
            ProcessConstraints(theModel);
        }

        private void ProcessConstraints(ModelModel theModel)
        {
            foreach (var constraint in theModel.Constraints)
            {
                var expressionConstraint = constraint as ExpressionConstraintModel;
                if (expressionConstraint == null)
                {
                    HandleAllDifferentConstraint((AllDifferentConstraintModel)constraint);
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
                    HandleEqualsOperator(expressionConstraint);
                    break;

                case OperatorType.NotEqual:
                    HandleNotEqualOperator(expressionConstraint);
                    break;

                case OperatorType.GreaterThanOrEqual:
                    HandleGreaterThanOrEqualOperator(expressionConstraint);
                    break;

                case OperatorType.LessThanOrEqual:
                    HandleLessThanOrEqualOperator(expressionConstraint);
                    break;

                case OperatorType.Greater:
                    HandleGreaterOperator(expressionConstraint);
                    break;

                case OperatorType.Less:
                    HandleLessOperator(expressionConstraint);
                    break;

                default:
                    throw new NotImplementedException("Not sure how to represent this operator type.");
            }
        }

        private IntVarVector ProcessVariables(ModelModel theModel)
        {
            var newVariables = new IntVarVector();
            ProcessSingletonVariables(theModel, newVariables);
            ProcessAggregateVariables(theModel, newVariables);

            return newVariables;
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
                var orVariable = ProcessVariable(variables, variable);
                this.singletonVariableMap.Add(variable.Name,
                                              new Tuple<VariableModel, IntVar>(variable, orVariable));
            }
        }

        private IntVar ProcessVariable(IntVarVector variables, VariableModel variable)
        {
            var band = GetVariableBand(variable);
            var orVariable = solver.MakeIntVar(band.Item1, band.Item2, variable.Name);
            variables.Add(orVariable);

            return orVariable;
        }

        private Tuple<long, long> GetVariableBand(VariableModel theVariable)
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
            var lhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
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
            var lhsExpr = GetExpressionFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsExpression)
            {
                var rhsExpr = GetExpressionFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                notEqualConstraint = this.solver.MakeNonEquality(lhsExpr, rhsExpr);
            }
            else if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
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
            var lhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
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
            var lhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
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
            var lhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
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
            var lhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.LeftExpression);
            if (constraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraint.Expression.Node.InnerExpression.RightExpression);
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable, rhsVariable);
            }
            else
            {
                greaterThanOrEqualConstraint = this.solver.MakeGreaterOrEqual(lhsVariable, constraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(greaterThanOrEqualConstraint);
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
                    variableExpression = GetVariableFrom((SingletonVariableReferenceExpressionNode)theExpression.InnerExpression);
                    op = singletonExpression.Operator;
                    literal = singletonExpression.Literal;
                }
                else
                {
                    var aggregateExpression = (AggregateVariableReferenceExpressionNode)theExpression.InnerExpression;
                    variableExpression = GetVariableFrom((AggregateVariableReferenceExpressionNode)theExpression.InnerExpression);
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
                var singletonVariableReference = (SingletonVariableReferenceNode)theExpression.InnerExpression;
                return GetSingletonVariableByName(singletonVariableReference.VariableName);
            }

            var aggregateVariableReference = (AggregateVariableReferenceNode)theExpression.InnerExpression;
            return GetAggregateVariableByName(aggregateVariableReference.VariableName,
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

        private IntVar GetSingletonVariableByName(string theVariableName)
        {
            return this.singletonVariableMap[theVariableName].Item2;
        }

        private IntVar GetAggregateVariableByName(string theVariableName, int index)
        {
            var orVariables = this.aggregateVariableMap[theVariableName].Item2;
            return orVariables[index - 1];
        }
    }
}
