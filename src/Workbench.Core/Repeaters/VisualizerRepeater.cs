using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal class VisualizerRepeater
    {
        private VisualizerRepeaterContext context;
        private readonly SolutionSnapshot snapshot;

        public VisualizerRepeater(SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            this.snapshot = theSnapshot;
        }

        public void Process(VisualizerRepeaterContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            this.context = theContext;
            if (theContext.Binding.Node.InnerExpression is MultiRepeaterStatementNode repeaterStatement)
            {
                var statementNode = repeaterStatement.Statement;
                if (!this.context.HasRepeaters)
                {
                    ProcessStatement(statementNode);
                }
                else
                {
                    while (this.context.Next())
                    {
                        ProcessStatement(statementNode);
                    }
                }
            }
            else
            {
                var statementListNode = (StatementListNode) theContext.Binding.Node.InnerExpression;
                foreach (var x in statementListNode.Statements)
                {
                    ProcessStatement(x);
                }
            }
        }

        public VisualizerRepeaterContext CreateContextFrom(VisualizerUpdateContext theContext)
        {
            return new VisualizerRepeaterContext(theContext);
        }

        private void ProcessStatement(StatementNode statementNode)
        {
            Contract.Requires<ArgumentNullException>(statementNode != null);
            if (statementNode.IsIfStatement)
            {
                ProcessIfStatement((IfStatementNode) statementNode.InnerStatement);
            }
            else if (statementNode.IsBindingStatement)
            {
                ProcessCallStatement((CallStatementNode) statementNode.InnerStatement);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void ProcessCallStatement(CallStatementNode theCallStatementNode)
        {
            Contract.Requires<ArgumentNullException>(theCallStatementNode != null);
            var visualizer = this.context.GetVisualizerByName(theCallStatementNode.VizualizerName.Name);
            var visualizerCall = CreateVisualizerCallFrom(theCallStatementNode);
            visualizer.UpdateWith(visualizerCall);
        }

        private void ProcessIfStatement(IfStatementNode ifStatementNode)
        {
            Contract.Requires<ArgumentNullException>(ifStatementNode != null);
            if (EvaluateIfStatementCondition(ifStatementNode.Expression))
            {
                ProcessCallStatement(ifStatementNode.Statement);
            }
        }

        private bool EvaluateIfStatementCondition(VisualizerBinaryExpressionNode binaryExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(binaryExpressionNode != null);

            var leftValue = EvaluateExpression(binaryExpressionNode.LeftExpression);
            var rightValue = EvaluateExpression(binaryExpressionNode.RightExpression);

            return EvaluateBinaryExpression(binaryExpressionNode.Operator, leftValue, rightValue);
        }

        private object EvaluateExpression(VisualizerExpressionNode theExpression)
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);

            switch (theExpression.InnerExpression)
            {
                case NumberLiteralNode numberLiteralNode:
                    return numberLiteralNode.Value;

                case ValueReferenceStatementNode valueReferenceStatementNode:
                    return EvaluateValueReferenceExpression(valueReferenceStatementNode);

                case CounterReferenceNode counterReferenceNode:
                    var counterContext = this.context.GetCounterContextByName(counterReferenceNode.CounterName);
                    return counterContext.CurrentValue;

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
                var singletonValue = this.snapshot.GetLabelByVariableName(singletonVariableName.Name);
                return singletonValue.Value;
            }
            else if (theValueReferenceExpression.IsAggregateValue)
            {
                var aggregateVariableName = theValueReferenceExpression.VariableName;
                var aggregateVariableOffset = theValueReferenceExpression.ValueOffset;
                int offsetValue;
                if (aggregateVariableOffset.IsLiteral)
                    offsetValue = aggregateVariableOffset.Literal.Value;
                else
                {
                    Contract.Assert(aggregateVariableOffset.IsCounterReference);
                    var counterReference = aggregateVariableOffset.Counter;
                    var counterContext = this.context.GetCounterContextByName(counterReference.CounterName);
                    offsetValue = counterContext.CurrentValue;
                }
                var aggregateValue = this.snapshot.GetCompoundLabelByVariableName(aggregateVariableName.Name);
                return aggregateValue.GetValueAt(offsetValue - 1);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private VisualizerCall CreateVisualizerCallFrom(CallStatementNode theCallStatementNode)
        {
            var arguments = new List<CallArgument>();
            foreach (var anArgument in theCallStatementNode.Arguments.Arguments)
            {
                var value = ConvertToValueFrom(anArgument);
                arguments.Add(new CallArgument(anArgument.Name.Name, value));
            }
            return new VisualizerCall(arguments);
        }

        private string ConvertToValueFrom(CallArgumentNode theArgument)
        {
            bool isNumeric = int.TryParse(theArgument.Value.GetValue(), out int n);
            if (isNumeric) return theArgument.Value.GetValue();
            if (theArgument.Name.Name == "x" || theArgument.Name.Name == "y")
            {
                var counterReference = this.context.GetCounterContextByName(theArgument.Value.GetValue());
                return Convert.ToString(counterReference.CurrentValue);
            }
            else if (theArgument.Value.Inner is ValueReferenceStatementNode valueReferenceNode)
            {
                return Convert.ToString(EvaluateValueReferenceExpression(valueReferenceNode));
            }
            else
            {
                return theArgument.Value.GetValue();
            }
        }

        private bool EvaluateBinaryExpression(OperatorType theOperator, object leftValue, object rightValue)
        {
            switch (leftValue)
            {
                case int _:
                    return EvaluateNumberBinaryExpression(theOperator, Convert.ToInt32(leftValue), rightValue);

                case char _:
                    return EvaluateCharBinaryExpression(theOperator, Convert.ToChar(leftValue), rightValue);

                case string _:
                    return EvaluateStringBinaryExpression(theOperator, Convert.ToString(leftValue), rightValue);

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
