using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    internal class DomainExpressionEvaluator
    {
        public static DomainValue Evaluate(RangeDomainExpressionNode theExpressionNode, ModelModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);
            Contract.Requires<ArgumentNullException>(theModel != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateNode(theExpressionNode, theModel);
        }

        public static DomainValue Evaluate(ListDomainExpressionNode theExpressionNode)
        {
            Contract.Requires<ArgumentNullException>(theExpressionNode != null);

            var evaluator = new DomainExpressionEvaluator();
            return evaluator.EvaluateNode(theExpressionNode);
        }

        public static DomainValue Evaluate(SharedDomainReferenceNode theExpressionNode, ModelModel theModel)
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
            return new RangeDomainValue(lhsBand, rhsBand);
        }

        private DomainValue EvaluateNode(ListDomainExpressionNode theExpressionNode)
        {
            var valueList = new List<string>();
            foreach (var itemNameNode in theExpressionNode.Items.Values)
            {
                valueList.Add(itemNameNode.Value);
            }
            return new ListDomainValue(valueList);
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
            if (theExpression.Inner is NumberLiteralNode numberLiteral)
            {
                return numberLiteral.Value;
            }

            if (theExpression.Inner is FunctionInvocationNode functionCall)
            {
                return EvaluateSizeFunction(functionCall, theModel);
            }

            throw new NotImplementedException("Unknown band expression node.");
        }

        private long EvaluateSizeFunction(FunctionInvocationNode functionCall, ModelModel theModel)
        {
            Debug.Assert(functionCall.FunctionName == "size");

            var variableName = functionCall.ArgumentList.Arguments.First().Value.Value;
            var theVariable = theModel.GetVariableByName(variableName);
            return theVariable.GetSize();
        }
    }
}