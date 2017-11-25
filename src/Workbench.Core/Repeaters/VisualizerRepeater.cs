using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;
using Workbench.Core.Solver;

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
            if (theContext.Binding.Node.InnerExpression is MultiRepeaterStatementNode)
            {
                var repeaterStatement = (MultiRepeaterStatementNode)theContext.Binding.Node.InnerExpression;
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
            var leftNumber = Convert.ToInt32(leftValue);
            var rightValue = EvaluateExpression(binaryExpressionNode.RightExpression);
            var rightNumber = Convert.ToInt32(rightValue);

            switch (binaryExpressionNode.Operator)
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

        private object EvaluateExpression(VisualizerExpressionNode theExpression)
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            if (theExpression.IsLiteral) return theExpression.GetLiteral();
            if (theExpression.IsValueReferenceExpression)
            {
                return EvaluateValueReferenceExpression(theExpression.ValueReference);
            }
            else if (theExpression.IsCounterReferenceExpression)
            {
                var counterReference = theExpression.CounterReference;
                var counterContext = this.context.GetCounterContextByName(counterReference.CounterName);
                return counterContext.CurrentValue;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private object EvaluateValueReferenceExpression(ValueReferenceStatementNode theValueReferenceExpression)
        {
            if (theValueReferenceExpression.IsSingletonValue)
            {
                var singletonVariableName = theValueReferenceExpression.VariableName;
                var singletonValue = this.snapshot.GetSingletonVariableValueByName(singletonVariableName.Name);
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
                var aggregateValue = this.snapshot.GetAggregateVariableValueByName(aggregateVariableName.Name);
                return aggregateValue.GetValueAt(offsetValue - 1).Model;
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
    }
}
