using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Range of values for a domain.
    /// </summary>
    public class RangeDomainValue : DomainValue
    {
        private RangeDomainExpressionNode expressionNode;

        /// <summary>
        /// Initialize a domain range with a low and high band.
        /// </summary>
        /// <param name="low">Low band.</param>
        /// <param name="high">High band.</param>
        internal RangeDomainValue(long low, long high, RangeDomainExpressionNode theNode)
            : base(theNode)
        {
            Lower = low;
            Upper = high;
            this.expressionNode = theNode;
        }

        /// <summary>
        /// Gets the lower band.
        /// </summary>
        public long Lower { get; private set; }

        /// <summary>
        /// Gets the upper band.
        /// </summary>
        public long Upper { get; private set; }

        public override Range GetRange()
        {
            return new Range(Lower, Upper);
        }

        /// <summary>
        /// Does the domain value intersect with this range.
        /// </summary>
        /// <param name="theDomainValue">Domain value.</param>
        /// <returns>True if intersects. False if it does not intersect.</returns>
        public override bool IntersectsWith(DomainValue theDomainValue)
        {
            Contract.Requires<ArgumentNullException>(theDomainValue != null);

            var otherModel = (RangeDomainValue) theDomainValue;
            return otherModel.Upper <= Upper && otherModel.Lower >= Lower;
        }

        /// <summary>
        /// Map from the solver value to the model value.
        /// </summary>
        /// <param name="solverValue">Solver value.</param>
        /// <returns>Model value.</returns>
        internal override object MapFrom(long solverValue)
        {
            if (IsNumberLiteralExpression())
            {
                return Lower + (solverValue - 1);
            }
            else if (IsCharacterLiteralExpression())
            {
                var leftCharacterNode = this.expressionNode.LeftExpression.Inner as CharacterLiteralNode;
                var rightCharacterNode = this.expressionNode.RightExpression.Inner as CharacterLiteralNode;
                var lowerCharacterLimit = leftCharacterNode.Value;
                var upperCharacterLimit = rightCharacterNode.Value;
                return Convert.ToChar(lowerCharacterLimit + (solverValue - 1));
            }
            else
            {
                throw new NotImplementedException("Unknown range expression.");
            }
        }

        /// <summary>
        /// Map from the model value to the solver value.
        /// </summary>
        /// <param name="modelValue">Model value.</param>
        /// <returns>Solver value.</returns>
        internal override int MapTo(object modelValue)
        {
            if (IsNumberLiteralExpression())
            {
                // Model value and the solver value are the same
                return Convert.ToInt32(modelValue);
            }
            else if (IsCharacterLiteralExpression())
            {
                var leftCharacterNode = this.expressionNode.LeftExpression.Inner as CharacterLiteralNode;
                var rightCharacterNode = this.expressionNode.RightExpression.Inner as CharacterLiteralNode;
                var lowerCharacterLimit = leftCharacterNode.Value;
                return (Convert.ToChar(modelValue) - lowerCharacterLimit) + 1;
            }
            else
            {
                throw new NotImplementedException("Unknown range expression.");
            }
        }

        private bool IsCharacterLiteralExpression()
        {
            return this.expressionNode.LeftExpression.Inner is CharacterLiteralNode;
        }

        private bool IsNumberLiteralExpression()
        {
            return this.expressionNode.LeftExpression.Inner is NumberLiteralNode;
        }
    }
}
