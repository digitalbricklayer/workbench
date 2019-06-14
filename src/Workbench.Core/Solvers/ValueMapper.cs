using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Map to / from domain and solver values.
    /// </summary>
    internal class ValueMapper
    {
        private readonly Dictionary<string, DomainValue> valueVariableDictionary;
        private readonly Dictionary<string, DomainValue> valueBucketMap;

        internal ValueMapper()
        {
            valueVariableDictionary = new Dictionary<string, DomainValue>();
            valueBucketMap = new Dictionary<string, DomainValue>();
        }

        internal DomainValue GetDomainValueFor(BucketVariableModel theBucket)
        {
            Contract.Requires<ArgumentNullException>(theBucket != null);

            return this.valueBucketMap[theBucket.Name];
        }

        internal DomainValue GetDomainValueFor(VariableModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);

            return this.valueVariableDictionary[theVariable.Name.Text];
        }

        internal void AddVariableDomainValue(SingletonVariableModel theSingleton, DomainValue theVariableBand)
        {
            Contract.Requires<ArgumentNullException>(theSingleton != null);
            Contract.Requires<ArgumentNullException>(theVariableBand != null);

            this.valueVariableDictionary.Add(theSingleton.Name.Text, theVariableBand);
        }

        internal void AddVariableDomainValue(AggregateVariableModel theAggregate, DomainValue theVariableBand)
        {
            Contract.Requires<ArgumentNullException>(theAggregate != null);
            Contract.Requires<ArgumentNullException>(theVariableBand != null);

            foreach (var variableModel in theAggregate.Variables)
            {
                this.valueVariableDictionary.Add(variableModel.Name.Text, theVariableBand);
            }
#if false
            this.valueVariableDictionary.Add(theAggregate.Name.Text, theVariableBand);
#endif
        }

        internal void AddBucketDomainValue(BucketVariableModel bucket, DomainValue variableBand)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Contract.Requires<ArgumentNullException>(variableBand != null);

            if (!this.valueBucketMap.ContainsKey(bucket.Name))
                this.valueBucketMap.Add(bucket.Name, variableBand);
        }
    }
}
