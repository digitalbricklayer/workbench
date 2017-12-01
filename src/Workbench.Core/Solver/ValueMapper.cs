using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// Map to / from domain and solver values.
    /// </summary>
    internal class ValueMapper
    {
        private Dictionary<string, DomainValue> valueVariableDictionary = new Dictionary<string, DomainValue>();

        internal DomainValue GetDomainValueFor(VariableGraphicModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);

            return this.valueVariableDictionary[theVariable.Name];
        }

#if false
        internal DomainValue GetDomainValueFor(AggregateVariableGraphicModel theVariable)
        {
            return this.valueVariableDictionary[theVariable.Name];
        }
#endif

        internal void AddVariableDomainValue(SingletonVariableGraphicModel theSingleton, DomainValue theVariableBand)
        {
            Contract.Requires<ArgumentNullException>(theSingleton != null);
            Contract.Requires<ArgumentNullException>(theVariableBand != null);

            this.valueVariableDictionary.Add(theSingleton.Name, theVariableBand);
        }

        internal void AddVariableDomainValue(AggregateVariableGraphicModel theAggregate, DomainValue theVariableBand)
        {
            Contract.Requires<ArgumentNullException>(theAggregate != null);
            Contract.Requires<ArgumentNullException>(theVariableBand != null);

            this.valueVariableDictionary.Add(theAggregate.Name, theVariableBand);
        }
    }
}
