using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
#if false
    /// <summary>
    /// A node containing an aggregate variable expression.
    /// </summary>
    internal sealed class AggregateVariableExpressionNode : Node
    {
        private readonly AggregateVariableReferenceExpressionNode _aggregateVariableReferenceExpression;
        private readonly IntegerVariable _variable;
        private readonly VariableExpressionOperatorType _operator;
        private readonly InfixStatementNode _infixStatement;

        internal AggregateVariableExpressionNode(AggregateVariableReferenceExpressionNode aggregateVariableReferenceExpression,
            IntegerVariable variable,
            VariableExpressionOperatorType op,
            InfixStatementNode infixStatement)
        {
            Contract.Requires<ArgumentNullException>(aggregateVariableReferenceExpression != null);
            Contract.Requires<ArgumentNullException>(variable != null);
            Contract.Requires<ArgumentNullException>(infixStatement != null);

            _aggregateVariableReferenceExpression = aggregateVariableReferenceExpression;
            _variable = variable;
            _operator = op;
            _infixStatement = infixStatement;
        }

        internal override bool IsSolved()
        {
            return !_variable.Domain.IsEmpty;
        }

        internal override DomainRange GetRange()
        {
            throw new NotImplementedException();
        }

#if false
        internal override int EvaluateWith(int value)
        {

        }
#endif 
    }
#endif
}