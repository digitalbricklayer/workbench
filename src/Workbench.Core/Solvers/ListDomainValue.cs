using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Nodes;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// List of values for a domain.
    /// </summary>
    public class ListDomainValue : DomainValue
    {
        private readonly List<string> values;
        private readonly ListDomainExpressionNode expressionNode;

        /// <summary>
        /// Initialize a domain range with a list of values.
        /// </summary>
        /// <param name="theList">List of values from the domain list.</param>
        /// <param name="theNode">Domain list node.</param>
        internal ListDomainValue(IEnumerable<string> theList, ListDomainExpressionNode theNode)
            : base()
        {
            Contract.Requires<ArgumentNullException>(theList != null);
            Contract.Requires<ArgumentNullException>(theNode != null);

            this.values = new List<string>(theList);
            this.expressionNode = theNode;
        }

        /// <summary>
        /// Gets the values from the domain list.
        /// </summary>
        public IReadOnlyList<string> Values
        {
            get { return new ReadOnlyCollection<string>(this.values); }
        }

        public override Range GetRange()
        {
            if (!Values.Any()) return new Range(0, 0);
            return new Range(1, Values.Count());
        }

        /// <summary>
        /// Does the domain value intersect with this range.
        /// </summary>
        /// <param name="theDomainValue">Domain value.</param>
        /// <returns>True if intersects. False if it does not intersect.</returns>
        public override bool IntersectsWith(DomainValue theDomainValue)
        {
            var otherModel = (ListDomainValue) theDomainValue;
#if false
            return otherModel.Upper <= Upper && otherModel.Lower >= Lower;
#else
            return false;
#endif
        }

        /// <summary>
        /// Map from the solver value to the model value.
        /// </summary>
        /// <param name="solverValue">Solver value.</param>
        /// <returns>Model value.</returns>
        internal override object MapFrom(long solverValue)
        {
            Contract.Assume(solverValue <= this.values.Count);
            return this.values[Convert.ToInt32(solverValue) - 1];
        }

        /// <summary>
        /// Map from the model value to the solver value.
        /// </summary>
        /// <param name="modelValue">Model value.</param>
        /// <returns>Solver value.</returns>
        internal override int MapTo(object modelValue)
        {
            var zeroBasedIndexOfModelValue = this.values.IndexOf(Convert.ToString(modelValue));
            return zeroBasedIndexOfModelValue + 1;
        }
    }
}
