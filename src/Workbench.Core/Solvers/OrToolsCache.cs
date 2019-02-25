using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Cache references between items in Google or-tools and their equivalents in the model.
    /// </summary>
    internal class OrToolsCache
    {
        private readonly Dictionary<string, Tuple<SingletonVariableModel, IntVar>> singletonVariableMap;
        private readonly Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> aggregateVariableMap;
        private readonly Dictionary<string, BucketVariableMap> bucketMap;

        internal OrToolsCache()
        {
            this.singletonVariableMap = new Dictionary<string, Tuple<SingletonVariableModel, IntVar>>();
            this.aggregateVariableMap = new Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>>();
            this.bucketMap = new Dictionary<string, BucketVariableMap>();
            Variables = new IntVarVector();
        }

        internal Dictionary<string, Tuple<SingletonVariableModel, IntVar>> SingletonVariableMap => this.singletonVariableMap;
        internal Dictionary<string, Tuple<AggregateVariableModel, IntVarVector>> AggregateVariableMap => this.aggregateVariableMap;
        internal IntVarVector Variables { get; private set; }
        internal Dictionary<string, BucketVariableMap> BucketMap => this.bucketMap;

        internal void AddVariable(IntVar orVar)
        {
            Variables.Add(orVar);
        }

        internal void AddAggregate(string name, Tuple<AggregateVariableModel, IntVarVector> tuple)
        {
            this.aggregateVariableMap.Add(name, tuple);
        }

        internal void AddSingleton(string name, Tuple<SingletonVariableModel, IntVar> tuple)
        {
            this.singletonVariableMap.Add(name, tuple);
        }

        internal void AddBucket(string name, BucketVariableMap variableMap)
        {
            this.bucketMap.Add(name, variableMap);
        }

        internal IntVarVector GetVectorByName(string aggregateName)
        {
            return this.aggregateVariableMap[aggregateName].Item2;
        }

        internal IntVar GetSingletonVariableByName(string theVariableName)
        {
            return this.singletonVariableMap[theVariableName].Item2;
        }

        internal IntVar GetAggregateVariableByName(string theVariableName, int index)
        {
            var orVariables = this.aggregateVariableMap[theVariableName].Item2;
            return orVariables[index];
        }

        internal IntVar GetBucketVariableByName(string bucketName, int index, string variableName)
        {
            var bucketVariableMap = this.bucketMap[bucketName];
            var bundleMap = bucketVariableMap.GetBundleVariableAt(index);
            var variableMap = bundleMap.GetVariableByName(variableName);
            return variableMap.SolverVariable;
        }
    }
}
