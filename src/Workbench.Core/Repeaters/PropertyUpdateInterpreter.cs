using System;
using System.Diagnostics;
using Irony.Interpreter.Ast;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Interpreter for the visualizer property update language.
    /// </summary>
    internal class PropertyUpdateInterpreter
    {
        private readonly SolutionSnapshot _snapshot;

        internal PropertyUpdateInterpreter(PropertyUpdateContext theContext)
        {
            _snapshot = theContext.Snapshot;
        }

        internal object Process(PropertyUpdateExpressionModel theExpression)
        {
            switch (theExpression.Node.InnerExpression)
            {
                case StatementNode statementNode:
                    return ProcessStatement(statementNode);

                default:
                    throw new NotImplementedException("Unknown property update statement type.");
            }
        }

        private object ProcessStatement(StatementNode statementNode)
        {
            switch (statementNode.InnerStatement)
            {
                case IfStatementNode ifStatement:
                    return ProcessIfStatement(ifStatement);

                case VisualizerExpressionNode expression:
                    return ProcessExpression(expression.InnerExpression);

                default:
                    throw new NotImplementedException("Unknown statement not implemented.");
            }
        }

        private object ProcessIfStatement(IfStatementNode ifStatementNode)
        {
            if (EvaluateIfStatementCondition(ifStatementNode.Expression))
            {
                return ProcessExpression(ifStatementNode.Statement);
            }

            return string.Empty;
        }

        private object ProcessExpression(AstNode statement)
        {
            switch (statement)
            {
                case ValueReferenceStatementNode valueReferenceStatement:
                    return EvaluateValueReferenceExpression(valueReferenceStatement);

                case IntegerLiteralNode numberLiteral:
                    return Convert.ToString(numberLiteral.Value);

                case CharacterLiteralNode characterLiteral:
                    return Convert.ToString(characterLiteral.Value);

                default:
                    throw new NotImplementedException("Unknown statement.");
            }
        }

        private bool EvaluateIfStatementCondition(VisualizerBinaryExpressionNode binaryExpressionNode)
        {
            var leftValue = EvaluateExpression(binaryExpressionNode.LeftExpression);
            var rightValue = EvaluateExpression(binaryExpressionNode.RightExpression);

            return EvaluateBinaryExpression(binaryExpressionNode.Operator, leftValue, rightValue);
        }

        private object EvaluateExpression(VisualizerExpressionNode theExpression)
        {
            switch (theExpression.InnerExpression)
            {
                case NumberLiteralNode numberLiteralNode:
                    return numberLiteralNode.Value;

                case ValueReferenceStatementNode valueReferenceStatementNode:
                    return EvaluateValueReferenceExpression(valueReferenceStatementNode);

                case ItemNameNode itemNameNode:
                    return itemNameNode.Value;

                case CharacterLiteralNode characterLiteralNode:
                    return characterLiteralNode.Value;

                default:
                    throw new NotImplementedException("Unknown visualizer expression");
            }
        }

        private object EvaluateValueReferenceExpression(ValueReferenceStatementNode theValueReferenceExpression)
        {
            if (theValueReferenceExpression.IsSingletonValue)
            {
                var singletonVariableName = theValueReferenceExpression.VariableName;
                var singletonValue = _snapshot.GetSingletonLabelByVariableName(singletonVariableName.Name);
                return singletonValue.Value;
            }
            else if (theValueReferenceExpression.IsAggregateValue)
            {
                var aggregateVariableName = theValueReferenceExpression.VariableName;
                var aggregateVariableOffset = theValueReferenceExpression.ValueOffset;
                Debug.Assert(aggregateVariableOffset.IsLiteral);
                var offsetValue = aggregateVariableOffset.Literal.Value;
                var aggregateValue = _snapshot.GetAggregateLabelByVariableName(aggregateVariableName.Name);
                return aggregateValue.GetValueAt(offsetValue - 1);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private bool EvaluateBinaryExpression(OperatorType theOperator, object leftValue, object rightValue)
        {
            switch (leftValue)
            {
                case int leftValueAsInt:
                    return EvaluateNumberBinaryExpression(theOperator, leftValueAsInt, rightValue);

                case char leftValueAsChar:
                    return EvaluateCharBinaryExpression(theOperator, leftValueAsChar, rightValue);

                case string leftValueAsString:
                    return EvaluateStringBinaryExpression(theOperator, leftValueAsString, rightValue);

                default:
                    throw new NotImplementedException("Unknown value type.");
            }
        }

        private bool EvaluateStringBinaryExpression(OperatorType theOperator, string leftString, object rightValue)
        {
            var rightString = rightValue.ToString();

            switch (theOperator)
            {
                case OperatorType.Equals:
                    return leftString == rightString;

                case OperatorType.NotEqual:
                    return leftString != rightString;

                default:
                    throw new NotImplementedException("Unknown operator.");
            }
        }

        private bool EvaluateCharBinaryExpression(OperatorType theOperator, int leftChar, object rightValue)
        {
            if (!char.TryParse(rightValue.ToString(), out char rightChar)) return false;

            switch (theOperator)
            {
                case OperatorType.Equals:
                    return leftChar == rightChar;

                case OperatorType.NotEqual:
                    return leftChar != rightChar;

                case OperatorType.Greater:
                    return leftChar > rightChar;

                case OperatorType.GreaterThanOrEqual:
                    return leftChar >= rightChar;

                case OperatorType.Less:
                    return leftChar < rightChar;

                case OperatorType.LessThanOrEqual:
                    return leftChar <= rightChar;

                default:
                    throw new NotImplementedException("Unknown operator.");
            }
        }

        private bool EvaluateNumberBinaryExpression(OperatorType theOperator, int leftNumber, object rightValue)
        {
            if (!Int32.TryParse(rightValue.ToString(), out int rightNumber)) return false;

            switch (theOperator)
            {
                case OperatorType.Equals:
                    return leftNumber == rightNumber;

                case OperatorType.NotEqual:
                    return leftNumber != rightNumber;

                case OperatorType.Greater:
                    return leftNumber > rightNumber;

                case OperatorType.GreaterThanOrEqual:
                    return leftNumber >= rightNumber;

                case OperatorType.Less:
                    return leftNumber < rightNumber;

                case OperatorType.LessThanOrEqual:
                    return leftNumber <= rightNumber;

                default:
                    throw new NotImplementedException("Unknown operator.");
            }
        }
    }
}
