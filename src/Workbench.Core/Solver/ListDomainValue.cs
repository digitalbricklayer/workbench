using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// List of values for a domain.
    /// </summary>
    public class ListDomainValue : DomainValue
    {
        private IList<string> values;

        /// <summary>
        /// Initialize a domain range with a list of values.
        /// </summary>
        /// <param name="low">Low band.</param>
        /// <param name="high">High band.</param>
        internal ListDomainValue(IEnumerable<string> theList)
        {
            this.values = new List<string>(theList);
        }

        /// <summary>
        /// Initialize a domain range with a list of values.
        /// </summary>
        /// <param name="low">Low band.</param>
        /// <param name="high">High band.</param>
        internal ListDomainValue(params string[] theList)
        {
            this.values = new List<string>(theList);
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
            Contract.Requires<ArgumentNullException>(theDomainValue != null);

            var otherModel = (ListDomainValue) theDomainValue;
#if false
            return otherModel.Upper <= Upper && otherModel.Lower >= Lower;
#else
            return false;
#endif
        }
    }
}
