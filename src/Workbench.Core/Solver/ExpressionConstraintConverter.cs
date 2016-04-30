using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Convert the expression constraint model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    class ExpressionConstraintConverter
    {
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;

        public ExpressionConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            this.solver = theSolver;
            this.cache = theCache;
        }

        public void HandleExpressionConstraint(ExpressionConstraintModel expressionConstraint)
        {
            Contract.Requires<ArgumentNullException>(expressionConstraint != null);
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
                return this.cache.GetSingletonVariableByName(singletonVariableReference.VariableName);
            }

            var aggregateVariableReference = (AggregateVariableReferenceNode)theExpression.InnerExpression;
            return this.cache.GetAggregateVariableByName(aggregateVariableReference.VariableName, aggregateVariableReference.Subscript);
        }

        private IntExpr GetVariableFrom(AggregateVariableReferenceExpressionNode aggregateExpression)
        {
            return this.cache.GetAggregateVariableByName(aggregateExpression.VariableReference.VariableName, aggregateExpression.VariableReference.Subscript);
        }

        private IntExpr GetVariableFrom(SingletonVariableReferenceExpressionNode singletonExpression)
        {
            return this.cache.GetSingletonVariableByName(singletonExpression.VariableReference.VariableName);
        }
    }
}
