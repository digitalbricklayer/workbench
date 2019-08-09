using System.Collections.Generic;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Map to / from domain and solver values.
    /// </summary>
    internal sealed class OrangeValueMapper
    {
        private readonly Dictionary<string, DomainValue> valueVariableDictionary;
        private readonly Dictionary<string, DomainValue> valueBucketMap;

        internal OrangeValueMapper()
        {
            valueVariableDictionary = new Dictionary<string, DomainValue>();
            valueBucketMap = new Dictionary<string, DomainValue>();
        }

        internal DomainValue GetDomainValueFor(BucketVariableModel theBucket)
        {
            return this.valueBucketMap[theBucket.Name];
        }

        internal DomainValue GetDomainValueFor(VariableModel theVariable)
        {
            return this.valueVariableDictionary[theVariable.Name.Text];
        }

        internal void AddVariableDomainValue(SingletonVariableModel theSingleton, DomainValue theVariableBand)
        {
            this.valueVariableDictionary.Add(theSingleton.Name.Text, theVariableBand);
        }

        internal void AddVariableDomainValue(AggregateVariableModel theAggregate, DomainValue theVariableBand)
        {
            foreach (var variableModel in theAggregate.Variables)
            {
                this.valueVariableDictionary.Add(variableModel.Name.Text, theVariableBand);
            }
        }

        internal void AddBucketDomainValue(BucketVariableModel bucket, DomainValue variableBand)
        {
            if (!this.valueBucketMap.ContainsKey(bucket.Name))
                this.valueBucketMap.Add(bucket.Name, variableBand);
        }
    }
}
