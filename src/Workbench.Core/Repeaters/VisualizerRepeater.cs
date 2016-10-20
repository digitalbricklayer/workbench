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

            switch (binaryExpressionNode.Operator)
            {
                case OperatorType.Equals:
                    return leftValue == rightValue;

                case OperatorType.NotEqual:
                    return leftValue != rightValue;

                case OperatorType.Greater:
                    return leftValue > rightValue;

                case OperatorType.GreaterThanOrEqual:
                    return leftValue >= rightValue;

                case OperatorType.Less:
                    return leftValue < rightValue;

                case OperatorType.LessThanOrEqual:
                    return leftValue <= rightValue;

                default:
                    throw new NotImplementedException("Unknown operator.");
            }
        }

        private int EvaluateExpression(VisualizerExpressionNode theExpression)
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            if (theExpression.IsLiteral) return theExpression.GetLiteral();
            if (theExpression.IsValueReferenceExpression)
            {
                var valueReferenceExpression = theExpression.ValueReference;
                if (valueReferenceExpression.IsSingletonValue)
                {
                    var singletonVariableName = valueReferenceExpression.VariableName;
                    var singletonValue = this.snapshot.GetSingletonVariableValueByName(singletonVariableName.Name);
                    return singletonValue.Value;
                }
                else if (valueReferenceExpression.IsAggregateValue)
                {
                    var aggregateVariableName = valueReferenceExpression.VariableName;
                    var aggregateVariableOffset = valueReferenceExpression.ValueOffset;
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
                    return aggregateValue.GetValueAt(offsetValue - 1);
                }
                else
                {
                    throw new NotImplementedException();
                }
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

        private VisualizerCall CreateVisualizerCallFrom(CallStatementNode theCallStatementNode)
        {
            var arguments = new List<CallArgument>();
            foreach (var anArgument in theCallStatementNode.Arguments.Arguments)
            {
                var x = ConvertToValueFrom(anArgument);
                arguments.Add(new CallArgument(anArgument.Name.Name, x));
            }
            return new VisualizerCall(arguments);
        }

        private string ConvertToValueFrom(CallArgumentNode theArgument)
        {
            int n;
            bool isNumeric = int.TryParse(theArgument.Value.GetValue(), out n);
            if (isNumeric) return theArgument.Value.GetValue();
            if (theArgument.Name.Name == "x" || theArgument.Name.Name == "y")
            {
                var counterReference = this.context.GetCounterContextByName(theArgument.Value.GetValue());
                return Convert.ToString(counterReference.CurrentValue);
            }
            else
            {
                return theArgument.Value.GetValue();
            }
        }
    }
}
