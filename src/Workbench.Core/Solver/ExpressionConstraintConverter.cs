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

        public void ProcessConstraint(ExpressionConstraintModel expressionConstraint)
        {
            Contract.Requires<ArgumentNullException>(expressionConstraint != null);
            Constraint newConstraint;
            var lhsExpr = GetExpressionFrom(expressionConstraint.Expression.Node.InnerExpression.LeftExpression);
            if (expressionConstraint.Expression.Node.InnerExpression.RightExpression.IsExpression)
            {
                var rhsExpr = GetExpressionFrom(expressionConstraint.Expression.Node.InnerExpression.RightExpression);
                newConstraint = CreateConstraintBy(expressionConstraint.Expression.OperatorType, lhsExpr, rhsExpr);
            }
            else if (expressionConstraint.Expression.Node.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(expressionConstraint.Expression.Node.InnerExpression.RightExpression);
                newConstraint = CreateConstraintBy(expressionConstraint.Expression.OperatorType, lhsExpr, rhsVariable);
            }
            else
            {
                newConstraint = CreateConstraintBy(expressionConstraint.Expression.OperatorType,
                                                   lhsExpr,
                                                   expressionConstraint.Expression.Node.InnerExpression.RightExpression.GetLiteral());
            }
            this.solver.Add(newConstraint);
        }

        private Constraint CreateConstraintBy(OperatorType operatorType, IntExpr lhsExpr, IntExpr rhsExpr)
        {
            switch (operatorType)
            {
                case OperatorType.Equals:
                    return this.solver.MakeEquality(lhsExpr, rhsExpr);

                case OperatorType.NotEqual:
                    return this.solver.MakeNonEquality(lhsExpr, rhsExpr);

                case OperatorType.GreaterThanOrEqual:
                    return this.solver.MakeGreaterOrEqual(lhsExpr, rhsExpr);

                case OperatorType.LessThanOrEqual:
                    return this.solver.MakeLessOrEqual(lhsExpr, rhsExpr);

                case OperatorType.Greater:
                    return this.solver.MakeGreater(lhsExpr, rhsExpr);

                case OperatorType.Less:
                    return this.solver.MakeLess(lhsExpr, rhsExpr);

                default:
                    throw new NotImplementedException("Not sure how to represent this operator type.");
            }
        }

        private Constraint CreateConstraintBy(OperatorType operatorType, IntExpr lhsExpr, int rhs)
        {
            switch (operatorType)
            {
                case OperatorType.Equals:
                    return this.solver.MakeEquality(lhsExpr, rhs);

                case OperatorType.NotEqual:
                    return this.solver.MakeNonEquality(lhsExpr, rhs);

                case OperatorType.GreaterThanOrEqual:
                    return this.solver.MakeGreaterOrEqual(lhsExpr, rhs);

                case OperatorType.LessThanOrEqual:
                    return this.solver.MakeLessOrEqual(lhsExpr, rhs);

                case OperatorType.Greater:
                    return this.solver.MakeGreater(lhsExpr, rhs);

                case OperatorType.Less:
                    return this.solver.MakeLess(lhsExpr, rhs);

                default:
                    throw new NotImplementedException("Not sure how to represent this operator type.");
            }
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
