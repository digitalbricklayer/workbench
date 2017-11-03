using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    internal class VariableBandEvaluator
    {
        internal static DomainRange GetVariableBand(VariableModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Contract.Assert(!theVariable.DomainExpression.IsEmpty);

            if (theVariable.DomainExpression.InlineDomain != null)
            {
                return EvaluateInlineDomainExpression(theVariable);
            }

            return EvaluateSharedDomainExpression(theVariable);
        }

        /// <summary>
        /// The domain is specified by a domain expression inline with the variable
        /// </summary>
        /// <param name="theVariable">Variable with domain expression.</param>
        /// <returns>Tuple with upper and lower band.</returns>
        private static DomainRange EvaluateInlineDomainExpression(VariableModel theVariable)
        {
            var inlineDomain = theVariable.DomainExpression.InlineDomain;
            var evaluatorContext = new DomainExpressionEvaluatorContext(inlineDomain, theVariable.Model);
            return DomainExpressionEvaluator.Evaluate(evaluatorContext);
        }

        ///<summary>
        /// The domain is specified in a shared domain seperately and the
        /// inline expression references the shared domain
        ///</summary>
        /// <param name="theVariable">Variable with domain expression.</param>
        /// <returns>Tuple with upper and lower band.</returns>
        private static DomainRange EvaluateSharedDomainExpression(VariableModel theVariable)
        {
            var evaluatorContext = new SharedDomainExpressionEvaluatorContext(theVariable.DomainExpression.Node, theVariable.Model);
            return SharedDomainExpressionEvaluator.Evaluate(evaluatorContext);
        }
    }
}
