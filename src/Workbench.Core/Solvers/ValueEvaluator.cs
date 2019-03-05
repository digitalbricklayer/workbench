using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Evaluate domain values against a constraint expression.
    /// </summary>
    internal sealed class ValueEvaluator
    {
        private readonly IntegerVariable _leftVariable;
        private readonly IntegerVariable _rightVariable;
        private readonly ConstraintExpression _expression;

        internal ValueEvaluator(IntegerVariable left, IntegerVariable right, ConstraintExpression constraint)
        {
            _leftVariable = left;
            _rightVariable = right;
            _expression = constraint;
        }

        /// <summary>
        /// Evaluate the value to see if it is compatible with the constraint expression.
        /// </summary>
        /// <param name="leftValue">Possible value for the subject variable.</param>
        /// <returns>True if the value is compatible with the expression or False if it is not compatible.</returns>
        internal bool EvaluateLeft(int leftValue)
        {
            if (_expression.Node.HasExpander) throw new NotImplementedException();

            foreach (var otherPossibleValue in _rightVariable.Domain.PossibleValues)
            {
                switch (_expression.Node.InnerExpression.Operator)
                {
                    case OperatorType.Greater:
                        if (leftValue > otherPossibleValue)
                            return true;
                        break;

                    case OperatorType.GreaterThanOrEqual:
                        if (leftValue >= otherPossibleValue)
                            return true;
                        break;

                    case OperatorType.Less:
                        if (leftValue < otherPossibleValue)
                            return true;
                        break;

                    case OperatorType.LessThanOrEqual:
                        if (leftValue <= otherPossibleValue)
                            return true;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return false;
        }

        /// <summary>
        /// Evaluate the value to see if it is compatible with the constraint expression.
        /// </summary>
        /// <param name="rightValue">Possible value for the subject variable.</param>
        /// <returns>True if the value is compatible with the expression or False if it is not compatible.</returns>
        internal bool EvaluateRight(int rightValue)
        {
            if (_expression.Node.HasExpander) throw new NotImplementedException();

            foreach (var otherPossibleValue in _leftVariable.Domain.PossibleValues)
            {
                switch (_expression.Node.InnerExpression.Operator)
                {
                    case OperatorType.Greater:
                        if (otherPossibleValue > rightValue)
                            return true;
                        break;

                    case OperatorType.GreaterThanOrEqual:
                        if (otherPossibleValue >= rightValue)
                            return true;
                        break;

                    case OperatorType.Less:
                        if (otherPossibleValue < rightValue)
                            return true;
                        break;

                    case OperatorType.LessThanOrEqual:
                        if (otherPossibleValue <= rightValue)
                            return true;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return false;
        }
    }
}