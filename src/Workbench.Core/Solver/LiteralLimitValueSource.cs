using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Implementation of the limit value source for a literal.
    /// </summary>
    internal sealed class LiteralLimitValueSource : ILimitValueSource
    {
        private readonly int literalValue;

        /// <summary>
        /// Initialize the literal value source with a literal node.
        /// </summary>
        /// <param name="theNode">Literal literalValue.</param>
        public LiteralLimitValueSource(LiteralNode theNode)
        {
            Contract.Requires<ArgumentNullException>(theNode != null);
            this.literalValue = theNode.Value;
        }

        /// <summary>
        /// Initialize the literal value source with a literal literal.
        /// </summary>
        /// <param name="aLiteralLiteral">Literal value.</param>
        public LiteralLimitValueSource(int aLiteralLiteral)
        {
            this.literalValue = aLiteralLiteral;
        }

        /// <summary>
        /// Get the current value of the scope limit.
        /// </summary>
        /// <returns>Current scope limit value.</returns>
        public int GetValue()
        {
            return this.literalValue;
        }
    }
}