using System;
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

        internal Source GetLeftSource()
        {
            // The encapsulated variable sits on the left side of the left arc
            switch (LeftArc.Connector.Left.Content)
            {
                case SolverVariable solverVariable:
                    return new Source(solverVariable.Name, solverVariable.Domain.PossibleValues);

                default:
                    throw new NotImplementedException();
            }
        }

        internal Source GetRightSource()
        {
            // The encapsulated variable sits on the right side of the right arc
            switch (RightArc.Connector.Right.Content)
            {
                case SolverVariable solverVariable:
                    return new Source(solverVariable.Name, solverVariable.Domain.PossibleValues);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
