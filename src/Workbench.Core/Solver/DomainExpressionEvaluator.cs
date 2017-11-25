using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Evaluator for domain expressions.
    /// </summary>
    internal sealed class DomainExpressionEvaluator
    {
        /// <summary>
        /// Evaluate a range domain expression.
        /// </summary>
        /// <param name="theExpressionNode">The range expression node.</param>
        /// <param name="theModel">The model.</param>
        /// <returns>Domain value.</returns>
        internal static DomainValue Evaluate(RangeDomainExpressionNode theExpressionNode, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateNode(theExpressionNode, theModel);
        }

        /// <summary>
        /// Evaluate a list domain expression.
        /// </summary>
        /// <param name="theExpressionNode">List domain expression node.</param>
        /// <returns>Domain value.</returns>
        internal static DomainValue Evaluate(ListDomainExpressionNode theExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateNode(theExpressionNode);
        }

        /// <summary>
        /// Evaluate a shared domain reference.
        /// </summary>
        /// <param name="theExpressionNode">Shared domain reference node.</param>
        /// <param name="theModel">The model.</param>
        /// <returns>Domain value.</returns>
        internal static DomainValue Evaluate(SharedDomainReferenceNode theExpressionNode, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateReference(theExpressionNode, theModel);
        }

        private DomainValue EvaluateNode(RangeDomainExpressionNode theExpressionNode, ModelModel theModel)
        {
            var lhsBand = EvaluateBand(theExpressionNode.LeftExpression, theModel);
            var rhsBand = EvaluateBand(theExpressionNode.RightExpression, theModel);
            return new RangeDomainValue(lhsBand, rhsBand, theExpressionNode);
        }

        private DomainValue EvaluateNode(ListDomainExpressionNode theExpressionNode)
        {
            var valueList = new List<string>();
            foreach (var itemNameNode in theExpressionNode.Items.Values)
            {
                valueList.Add(itemNameNode.Value);
            }
            return new ListDomainValue(valueList, theExpressionNode);
        }

        private DomainValue EvaluateReference(SharedDomainReferenceNode theExpressionNode, ModelModel theModel)
        {
            var sharedDomainName = theExpressionNode.DomainName;
            var sharedDomainModel = theModel.GetSharedDomainByName(sharedDomainName.Name);

            var evaluatorContext = new SharedDomainExpressionEvaluatorContext(sharedDomainModel.Expression.Node, theModel);
            return SharedDomainExpressionEvaluator.Evaluate(evaluatorContext);
        }

        private long EvaluateBand(BandExpressionNode theExpression, ModelModel theModel)
        {
            switch (theExpression.Inner)
            {
                case NumberLiteralNode numberLiteral:
                    return numberLiteral.Value;

                case FunctionInvocationNode functionCall:
                    return EvaluateSizeFunction(functionCall, theModel);

                case CharacterLiteralNode characterLiteral:
                    return characterLiteral.Value - 'a' + 1;

                default:
                    throw new NotImplementedException("Unknown band expression node.");
            }
        }

        private long EvaluateSizeFunction(FunctionInvocationNode functionCall, ModelModel theModel)
        {
            Contract.Assert(functionCall.FunctionName == "size");

            var variableName = functionCall.ArgumentList.Arguments.First().Value.Value;
            var theVariable = theModel.GetVariableByName(variableName);
            return theVariable.GetSize();
        }
    }
}