using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;
using Workbench.Core.Solver;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Process a constraint repeater by expanding the expression an 
    /// appropriate number of times.
    /// </summary>
    internal class ConstraintRepeater
    {
        private ConstraintRepeaterContext context;
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly ModelModel model;
        private readonly ValueMapper valueMapper;

        internal ConstraintRepeater(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ModelModel theModel, ValueMapper theValueMapper)
        {
            Contract.Requires<ArgumentNullException>(theSolver != null);
            Contract.Requires<ArgumentNullException>(theCache != null);
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theValueMapper != null);

            this.solver = theSolver;
            this.cache = theCache;
            this.model = theModel;
            this.valueMapper = theValueMapper;
        }

        internal void Process(ConstraintRepeaterContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            this.context = theContext;
            if (!theContext.HasRepeaters)
            {
                ProcessSimpleConstraint(theContext.Constraint.Expression.Node);
            }
            else
            {
                var expressionTemplateWoutExpanderText = StripExpanderFrom(theContext.Constraint.Expression.Text);
                while (theContext.Next())
                {
                    var expressionText = InsertCounterValuesInto(expressionTemplateWoutExpanderText);
                    var expandedConstraintExpressionResult = new ConstraintExpressionParser().Parse(expressionText);
                    ProcessSimpleConstraint(expandedConstraintExpressionResult.Root);
                }
            }
        }

        internal ConstraintRepeaterContext CreateContextFrom(ExpressionConstraintModel constraint)
        {
            return new ConstraintRepeaterContext(constraint, this.model);
        }

        private void ProcessSimpleConstraint(ConstraintExpressionNode constraintExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(constraintExpressionNode != null);

            Constraint newConstraint;
            var lhsExpr = GetExpressionFrom(constraintExpressionNode.InnerExpression.LeftExpression);
            if (constraintExpressionNode.InnerExpression.RightExpression.IsExpression)
            {
                var rhsExpr = GetExpressionFrom(constraintExpressionNode.InnerExpression.RightExpression);
                newConstraint = CreateConstraintBy(constraintExpressionNode.InnerExpression.Operator, lhsExpr, rhsExpr);
            }
            else if (constraintExpressionNode.InnerExpression.RightExpression.IsVarable)
            {
                var rhsVariable = GetVariableFrom(constraintExpressionNode.InnerExpression.RightExpression);
                newConstraint = CreateConstraintBy(constraintExpressionNode.InnerExpression.Operator, lhsExpr, rhsVariable);
            }
            else if (constraintExpressionNode.InnerExpression.RightExpression.InnerExpression is IntegerLiteralNode)
            {
                newConstraint = CreateConstraintBy(constraintExpressionNode.InnerExpression.Operator,
                                                   lhsExpr,
                                                   constraintExpressionNode.InnerExpression.RightExpression.GetLiteral());
            }
            else if (constraintExpressionNode.InnerExpression.RightExpression.InnerExpression is ItemNameNode node)
            {
                var lhsVariableName = GetVariableNameFrom(constraintExpressionNode.InnerExpression.LeftExpression);
                var lhsVariable = this.model.GetVariableByName(lhsVariableName);
                var range = this.valueMapper.GetDomainValueFor(lhsVariable);
                var modelValue = GetModelValueFrom(node);
                var solverValue = range.MapTo(modelValue);
                newConstraint = CreateConstraintBy(constraintExpressionNode.InnerExpression.Operator, lhsExpr, solverValue);
            }
            else if (constraintExpressionNode.InnerExpression.RightExpression.InnerExpression is CharacterLiteralNode literalNode)
            {
                var lhsVariableName = GetVariableNameFrom(constraintExpressionNode.InnerExpression.LeftExpression);
                var lhsVariable = this.model.GetVariableByName(lhsVariableName);
                var range = this.valueMapper.GetDomainValueFor(lhsVariable);
                var modelValue = GetModelValueFrom(literalNode);
                var solverValue = range.MapTo(modelValue);
                newConstraint = CreateConstraintBy(constraintExpressionNode.InnerExpression.Operator, lhsExpr, solverValue);
            }
            else
            {
                throw new NotImplementedException("Unknown constraint expression.");
            }
            this.solver.Add(newConstraint);
        }

        private Constraint CreateConstraintBy(OperatorType operatorType, IntExpr lhsExpr, IntExpr rhsExpr)
        {
            switch (operatorType)
            {
                case OperatorType.Equals:
                    return this.solver.MakeEquality(lhsExpr, rhsExpr);

                case OperatorType.NotEqual:
                    return this.solver.MakeNonEquality(lhsExpr, rhsExpr);

                case OperatorType.GreaterThanOrEqual:
                    return this.solver.MakeGreaterOrEqual(lhsExpr, rhsExpr);

                case OperatorType.LessThanOrEqual:
                    return this.solver.MakeLessOrEqual(lhsExpr, rhsExpr);

                case OperatorType.Greater:
                    return this.solver.MakeGreater(lhsExpr, rhsExpr);

                case OperatorType.Less:
                    return this.solver.MakeLess(lhsExpr, rhsExpr);

                default:
                    throw new NotImplementedException("Not sure how to represent this operator type.");
            }
        }

        private Constraint CreateConstraintBy(OperatorType operatorType, IntExpr lhsExpr, int rhs)
        {
            switch (operatorType)
            {
                case OperatorType.Equals:
                    return this.solver.MakeEquality(lhsExpr, rhs);

                case OperatorType.NotEqual:
                    return this.solver.MakeNonEquality(lhsExpr, rhs);

                case OperatorType.GreaterThanOrEqual:
                    return this.solver.MakeGreaterOrEqual(lhsExpr, rhs);

                case OperatorType.LessThanOrEqual:
                    return this.solver.MakeLessOrEqual(lhsExpr, rhs);

                case OperatorType.Greater:
                    return this.solver.MakeGreater(lhsExpr, rhs);

                case OperatorType.Less:
                    return this.solver.MakeLess(lhsExpr, rhs);

                default:
                    throw new NotImplementedException("Not sure how to represent this operator type.");
            }
        }

        private IntExpr GetExpressionFrom(ExpressionNode theExpression)
        {
            if (theExpression.IsExpression)
            {
                IntExpr variableExpression;
                VariableExpressionOperatorType op;
                InfixStatementNode infixStatement;
                if (theExpression.IsSingletonExpression)
                {
                    var singletonExpression = (SingletonVariableReferenceExpressionNode)theExpression.InnerExpression;
                    variableExpression = GetVariableFrom((SingletonVariableReferenceExpressionNode)theExpression.InnerExpression);
                    op = singletonExpression.Operator;
                    infixStatement = singletonExpression.InfixStatement;
                }
                else
                {
                    var aggregateExpression = (AggregateVariableReferenceExpressionNode)theExpression.InnerExpression;
                    variableExpression = GetVariableFrom((AggregateVariableReferenceExpressionNode)theExpression.InnerExpression);
                    op = aggregateExpression.Operator;
                    infixStatement = aggregateExpression.InfixStatement;
                }

                switch (op)
                {
                    case VariableExpressionOperatorType.Add:
                        return this.solver.MakeSum(variableExpression, GetValueFrom(infixStatement));

                    case VariableExpressionOperatorType.Subtract:
                        return this.solver.MakeSum(variableExpression, -GetValueFrom(infixStatement));

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                return GetVariableFrom(theExpression);
            }
        }

        private IntVar GetVariableFrom(ExpressionNode theExpression)
        {
            Debug.Assert(!theExpression.IsLiteral);

            if (theExpression.IsSingletonReference)
            {
                var singletonVariableReference = (SingletonVariableReferenceNode)theExpression.InnerExpression;
                return this.cache.GetSingletonVariableByName(singletonVariableReference.VariableName);
            }

            var aggregateVariableReference = (AggregateVariableReferenceNode)theExpression.InnerExpression;
            return this.cache.GetAggregateVariableByName(aggregateVariableReference.VariableName, aggregateVariableReference.SubscriptStatement.Subscript);
        }

        private IntExpr GetVariableFrom(AggregateVariableReferenceExpressionNode aggregateExpression)
        {
            return this.cache.GetAggregateVariableByName(aggregateExpression.VariableReference.VariableName, aggregateExpression.VariableReference.SubscriptStatement.Subscript);
        }

        private IntExpr GetVariableFrom(SingletonVariableReferenceExpressionNode singletonExpression)
        {
            return this.cache.GetSingletonVariableByName(singletonExpression.VariableReference.VariableName);
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
    }
}
