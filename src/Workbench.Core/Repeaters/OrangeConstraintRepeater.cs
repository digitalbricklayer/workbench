using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;
using Workbench.Core.Solvers;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Process a constraint repeater by expanding the expression an 
    /// appropriate number of times.
    /// </summary>
    internal class OrangeConstraintRepeater
    {
        private OrangeConstraintRepeaterContext _context;
        private readonly OrangeModelSolverMap _modelSolverMap;
        private readonly ModelModel _model;
        private readonly ValueMapper _valueMapper;
        private readonly ConstraintNetwork _constraintNetwork;

        internal OrangeConstraintRepeater(ConstraintNetwork constraintNetwork, OrangeModelSolverMap modelSolverMap, ModelModel theModel, ValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(constraintNetwork != null);
            Contract.Requires<ArgumentNullException>(modelSolverMap != null);
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            _constraintNetwork = constraintNetwork;
            _modelSolverMap = modelSolverMap;
            _model = theModel;
            _valueMapper = theValueMapper;
        }

        internal void Process(OrangeConstraintRepeaterContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null);
            Contract.Assert(context.HasRepeaters);

            _context = context;
            var expressionTemplateWithoutExpanderText = StripExpanderFrom(context.Constraint.Expression.Text);
            while (context.Next())
            {
                var expressionText = InsertCounterValuesInto(expressionTemplateWithoutExpanderText);
                var expandedConstraintExpressionResult = new ConstraintExpressionParser().Parse(expressionText);
                ProcessConstraint(expandedConstraintExpressionResult.Root);
            }
        }

        internal OrangeConstraintRepeaterContext CreateContextFrom(ExpressionConstraintModel constraint)
        {
            return new OrangeConstraintRepeaterContext(constraint, _model);
        }

        private void ProcessConstraint(ConstraintExpressionNode constraintExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(constraintExpressionNode != null);

            var lhsVariable = GetExpressionFrom(constraintExpressionNode.InnerExpression.LeftExpression);
            Arc newArc;
            if (constraintExpressionNode.InnerExpression.RightExpression.IsVariable)
            {
                var rhsVariable = GetVariableFrom(constraintExpressionNode.InnerExpression.RightExpression);
                newArc = CreateArcFrom(constraintExpressionNode, lhsVariable, rhsVariable);
            }
            else if (constraintExpressionNode.InnerExpression.RightExpression.InnerExpression is IntegerLiteralNode integerLiteral)
            {
                newArc = CreateArcFrom(constraintExpressionNode, lhsVariable, integerLiteral);
            }
            else
            {
                throw new NotImplementedException("Constraint expression not implemented.");
            }
            _constraintNetwork.AddArc(newArc);
        }

        private IntegerVariable GetExpressionFrom(ExpressionNode theExpression)
        {
            Contract.Assume(!theExpression.IsExpression);
            return GetVariableFrom(theExpression);
        }

        private IntegerVariable GetVariableFrom(ExpressionNode theExpression)
        {
            Debug.Assert(!theExpression.IsLiteral);

            switch (theExpression.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReferenceNode:
                    return _modelSolverMap.GetSolverSingletonVariableByName(singletonVariableReferenceNode.VariableName);

                case AggregateVariableReferenceNode aggregateVariableReference:
                    return _modelSolverMap.GetSolverAggregateVariableByName(aggregateVariableReference.VariableName, aggregateVariableReference.SubscriptStatement.Subscript);

#if false
                case BucketVariableReferenceNode bucketVariableReference:
                    return cache.GetBucketVariableByName(bucketVariableReference.BucketName, bucketVariableReference.SubscriptStatement.Subscript, bucketVariableReference.VariableName);
#endif

                default:
                    throw new NotImplementedException("Unknown variable expression.");
            }
        }

        private string StripExpanderFrom(string expressionText)
        {
            var expanderKeywordPos = expressionText.IndexOf("|", StringComparison.Ordinal);
            var raw = expressionText.Substring(0, expanderKeywordPos);
            return raw.Trim();
        }

        private string InsertCounterValuesInto(string expressionTemplateText)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(expressionTemplateText));

            var accumulatingTemplateText = expressionTemplateText;
            foreach (var aCounter in _context.Counters)
            {
                accumulatingTemplateText = InsertCounterValueInto(accumulatingTemplateText,
                    aCounter.CounterName,
                    aCounter.CurrentValue);
            }

            return accumulatingTemplateText;
        }

        private string InsertCounterValueInto(string expressionTemplateText, string counterName, int counterValue)
        {
            return expressionTemplateText.Replace(counterName, Convert.ToString(counterValue));
        }

        private int GetValueFrom(InfixStatementNode infixStatement)
        {
            Contract.Requires<ArgumentNullException>(infixStatement != null);
            Contract.Requires<ArgumentException>(infixStatement.IsCounterReference ||
                                                 infixStatement.IsLiteral);
            if (infixStatement.IsLiteral) return infixStatement.Literal.Value;
            var counter = _context.GetCounterContextByName(infixStatement.CounterReference.CounterName);
            return counter.CurrentValue;
        }

        private object GetModelValueFrom(ItemNameNode theItemNameNode)
        {
            return theItemNameNode.Value;
        }

        private object GetModelValueFrom(CharacterLiteralNode theCharacterLiteralNode)
        {
            return theCharacterLiteralNode.Value;
        }

        private string GetVariableNameFrom(ExpressionNode lhsExpression)
        {
            switch (lhsExpression.InnerExpression)
            {
                case SingletonVariableReferenceNode singletonVariableReferenceNode:
                    return singletonVariableReferenceNode.VariableName;

                case AggregateVariableReferenceExpressionNode aggregateVariableReferenceNode:
                    return aggregateVariableReferenceNode.VariableReference.VariableName;

                default:
                    throw new NotImplementedException("Unknown variable reference type.");
            }
        }

        private Arc CreateArcFrom(ConstraintExpressionNode constraintExpressionNode, IntegerVariable lhs, IntegerLiteralNode integerLiteralNode)
        {
            var literal = GetLiteralFrom(integerLiteralNode);
            return new Arc(new VariableNode(lhs), new LiteralNode(literal), new NodeConnector(new ConstraintExpression(constraintExpressionNode)));
        }

        private Arc CreateArcFrom(ConstraintExpressionNode constraintExpressionNode, IntegerVariable lhs, IntegerVariable rhs)
        {
            return new Arc(new VariableNode(lhs), new VariableNode(rhs), new NodeConnector(new ConstraintExpression(constraintExpressionNode)));
        }

        private int GetLiteralFrom(IntegerLiteralNode integerLiteralNode)
        {
            return new LiteralLimitValueSource(integerLiteralNode).GetValue();
        }
    }
}
