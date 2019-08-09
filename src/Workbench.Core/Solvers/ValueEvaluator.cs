using System;
using System.Collections.Generic;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Evaluate domain values on the left side of an expression against a constraint expression.
    /// </summary>
    internal sealed class LeftValueEvaluator
    {
        private readonly BinaryConstraintExpression _expression;
        private readonly IReadOnlyCollection<int> _rightPossibleValues;

        internal LeftValueEvaluator(IReadOnlyCollection<int> rightPossibleValues, BinaryConstraintExpression constraint)
        {
            _rightPossibleValues = rightPossibleValues;
            _expression = constraint;
        }

        /// <summary>
        /// Evaluate the value to see if it is compatible with the constraint expression.
        /// </summary>
        /// <param name="leftValue">Possible value for the subject variable.</param>
        /// <returns>True if the value is compatible with the expression or False if it is not compatible.</returns>
        internal bool EvaluateLeft(int leftValue)
        {
            foreach (var otherPossibleValue in _rightPossibleValues)
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
    }

    /// <summary>
    /// Evaluate domain values on the right side of an expression against a constraint expression.
    /// </summary>
    internal sealed class RightValueEvaluator
    {
        private readonly IReadOnlyCollection<int> _leftPossibleValues;
        private readonly BinaryConstraintExpression _expression;

        internal RightValueEvaluator(IReadOnlyCollection<int> leftPossibleValues, BinaryConstraintExpression constraint)
        {
            _leftPossibleValues = leftPossibleValues;
            _expression = constraint;
        }

        /// <summary>
        /// Evaluate the value to see if it is compatible with the constraint expression.
        /// </summary>
        /// <param name="rightValue">Possible value for the subject variable.</param>
        /// <returns>True if the value is compatible with the expression or False if it is not compatible.</returns>
        internal bool EvaluateRight(int rightValue)
        {
            foreach (var otherPossibleValue in _leftPossibleValues)
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