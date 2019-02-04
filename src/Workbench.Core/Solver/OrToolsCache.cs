using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
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
    }

    internal sealed class BucketVariableMap
    {
        private readonly List<BucketMap> _variableMap;

        internal BucketVariableMap(BucketModel bucket)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Bucket = bucket;
            _variableMap = new List<BucketMap>();
        }

        internal BucketModel Bucket { get; }

        internal void Add(SingletonVariableModel singletonVariable, IntVar orVariable)
        {
            _variableMap.Add(new BucketMap(singletonVariable, orVariable));
        }

        internal IReadOnlyCollection<BucketMap> GetVariableMaps()
        {
            return new ReadOnlyCollection<BucketMap>(_variableMap);
        }
    }

    internal sealed class BucketMap
    {
        internal BucketMap(SingletonVariableModel modelVariable, IntVar solverVariable)
        {
            Contract.Requires<ArgumentNullException>(modelVariable != null);
            SolverVariable = solverVariable;
            ModelVariable = modelVariable;
        }

        internal IntVar SolverVariable { get; }
        internal SingletonVariableModel ModelVariable { get; }
    }
}
