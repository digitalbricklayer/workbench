using System;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
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
        public LiteralLimitValueSource(IntegerLiteralNode theNode)
        {
            this.literalValue = theNode.Value;
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