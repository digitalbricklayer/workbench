using System;
using System.Collections.Generic;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    internal sealed class TernaryConstraintExpression
    {
        internal TernaryConstraintExpression(ConstraintExpressionNode constraintExpressionModel, EncapsulatedVariableNode expressionNode, Arc leftArc, Arc rightArc)
        {
            ExpressionNode = constraintExpressionModel;
            Node = expressionNode;
            EncapsulatedVariable = expressionNode.Variable;
            LeftArc = leftArc;
            RightArc = rightArc;
        }

        internal EncapsulatedVariableNode Node { get; }

        internal EncapsulatedVariable EncapsulatedVariable { get; }

        internal Arc LeftArc { get; }

        internal Arc RightArc { get; }

        internal ConstraintExpressionNode ExpressionNode { get; set; }

        internal IEnumerable<int> GetLeftSource()
        {
            // The encapsulated variable sits on the right side of the left arc
            switch (LeftArc.Connector.Left.Content)
            {
                case SolverVariable integerVariable:
                    return integerVariable.Domain.PossibleValues;

                default:
                    throw new NotImplementedException();
            }
        }

        internal IEnumerable<int> GetRightSource()
        {
            // The encapsulated variable sits on the left side of the right arc
            switch (RightArc.Connector.Right.Content)
            {
                case SolverVariable integerVariable:
                    return integerVariable.Domain.PossibleValues;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
