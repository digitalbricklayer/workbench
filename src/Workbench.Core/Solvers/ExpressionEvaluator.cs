using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Evaluate one side of a constraint expressions.
    /// </summary>
    internal sealed class ExpressionEvaluator
    {
        private readonly OrangeModelSolverMap _modelSolverMap;

        /// <summary>
        /// Initialize an expression with a model to solver map.
        /// </summary>
        /// <param name="modelSolverMap">Model to solver map.</param>
        internal ExpressionEvaluator(OrangeModelSolverMap modelSolverMap)
        {
            Contract.Requires<ArgumentNullException>(modelSolverMap != null);
            _modelSolverMap = modelSolverMap;
        }

        /// <summary>
        /// Evaluate the expression.
        /// </summary>
        /// <param name="expression">Constraint expression.</param>
        /// <param name="possibleValue">Possible value to be assigned to a variable.</param>
        /// <returns>Processed value through the expression</returns>
        internal int Evaluate(ExpressionNode expression, int possibleValue)
        {
            if (!expression.IsExpression) return possibleValue;

            VariableExpressionOperatorType op;
            InfixStatementNode infixStatement;
            if (expression.IsSingletonExpression)
            {
                var singletonExpression = (SingletonVariableReferenceExpressionNode) expression.InnerExpression;
                op = singletonExpression.Operator;
                infixStatement = singletonExpression.InfixStatement;
            }
            else
            {
                var aggregateExpression = (AggregateVariableReferenceExpressionNode) expression.InnerExpression;
                op = aggregateExpression.Operator;
                infixStatement = aggregateExpression.InfixStatement;
            }

            switch (op)
            {
                case VariableExpressionOperatorType.Add:
                    return possibleValue + GetValueFrom(infixStatement);

                case VariableExpressionOperatorType.Subtract:
                    return possibleValue - GetValueFrom(infixStatement);

                default:
                    throw new NotImplementedException();
            }
        }

        private int GetValueFrom(InfixStatementNode infixStatement)
        {
            Contract.Requires<ArgumentNullException>(infixStatement != null);
            Contract.Assume(infixStatement.IsLiteral);

            return infixStatement.Literal.Value;
        }
    }
}