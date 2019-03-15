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
        private OrangeConstraintRepeaterContext context;
        private readonly OrangeCache cache;
        private readonly ModelModel model;
        private readonly ValueMapper valueMapper;
        private readonly ConstraintNetwork _constraintNetwork;

        internal OrangeConstraintRepeater(ConstraintNetwork constraintNetwork, OrangeCache cache, ModelModel theModel, ValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(constraintNetwork != null);
            Contract.Requires<ArgumentNullException>(cache != null);
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            _constraintNetwork = constraintNetwork;
            this.cache = cache;
            this.model = theModel;
            this.valueMapper = theValueMapper;
        }

        internal void Process(OrangeConstraintRepeaterContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            this.context = theContext;
            var expressionTemplateWithoutExpanderText = StripExpanderFrom(theContext.Constraint.Expression.Text);
            while (theContext.Next())
            {
                var expressionText = InsertCounterValuesInto(expressionTemplateWithoutExpanderText);
                var expandedConstraintExpressionResult = new ConstraintExpressionParser().Parse(expressionText);
                ProcessSimpleConstraint(expandedConstraintExpressionResult.Root);
            }
        }

        internal OrangeConstraintRepeaterContext CreateContextFrom(ExpressionConstraintModel constraint)
        {
            return new OrangeConstraintRepeaterContext(constraint, this.model);
        }

        private void ProcessSimpleConstraint(ConstraintExpressionNode constraintExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(constraintExpressionNode != null);

            var lhsVariable = GetExpressionFrom(constraintExpressionNode.InnerExpression.LeftExpression);
            Arc newArc;
            if (constraintExpressionNode.InnerExpression.RightExpression.IsVariable)
            {
                var rhsVariable = GetVariableFrom(constraintExpressionNode.InnerExpression.RightExpression);
                newArc = CreateConstraintBy(constraintExpressionNode, lhsVariable, rhsVariable);
            }
//            else if (constraintExpressionNode.InnerExpression.RightExpression.InnerExpression is IntegerLiteralNode integerLiteral)
//            {
//                newArc = CreateConstraintBy(constraintExpressionNode.InnerExpression.Operator, lhsExpr, integerLiteral);
//            }
            else
            {
                throw new NotImplementedException("Constraint expression not implemented.");
            }
            _constraintNetwork.AddArc(newArc);
        }

#if true
        private Arc CreateConstraintBy(ConstraintExpressionNode constraintExpressionNode, IntegerVariable lhs, IntegerVariable rhs)
        {
            return new Arc(lhs, rhs, new ConstraintExpression(constraintExpressionNode));
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
                    return this.cache.GetSolverSingletonVariableByName(singletonVariableReferenceNode.VariableName);

                case AggregateVariableReferenceNode aggregateVariableReference:
                    return this.cache.GetSolverAggregateVariableByName(aggregateVariableReference.VariableName, aggregateVariableReference.SubscriptStatement.Subscript);

#if false
                case BucketVariableReferenceNode bucketVariableReference:
                    return this.cache.GetBucketVariableByName(bucketVariableReference.BucketName, bucketVariableReference.SubscriptStatement.Subscript, bucketVariableReference.VariableName);
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
            foreach (var aCounter in this.context.Counters)
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
            var counter = context.GetCounterContextByName(infixStatement.CounterReference.CounterName);
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
                    throw new NotImplementedException("Unknown variable reference.");
            }
        }
#endif
    }
}
