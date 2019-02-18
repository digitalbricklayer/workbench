using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
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

    internal sealed class BucketVariableMap
    {
        private readonly List<BundleMap> _bundleMaps;

        internal BucketVariableMap(BucketVariableModel bucket)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Bucket = bucket;
            _bundleMaps = new List<BundleMap>();
        }

        internal BucketVariableModel Bucket { get; }

        internal IReadOnlyCollection<BundleMap> GetBundleMaps()
        {
            return new ReadOnlyCollection<BundleMap>(_bundleMaps);
        }

        internal void Add(BundleMap bundleMap)
        {
            _bundleMaps.Add(bundleMap);
        }

        internal BundleMap GetBundleVariableAt(int index)
        {
            return _bundleMaps.ElementAt(index);
        }
    }

    internal class BundleMap
    {
        private readonly List<SingletonVariableMap> _variableMap;

        internal BundleMap(BundleModel bundle)
        {
            Bundle = bundle;
            _variableMap = new List<SingletonVariableMap>();
        }

        internal BundleModel Bundle { get; }

        internal IReadOnlyCollection<SingletonVariableMap> GetVariableMaps()
        {
            return new ReadOnlyCollection<SingletonVariableMap>(_variableMap);
        }

        internal void Add(SingletonVariableModel singletonVariable, IntVar orVariable)
        {
            _variableMap.Add(new SingletonVariableMap(singletonVariable, orVariable));
        }

        internal SingletonVariableMap GetVariableByName(string variableName)
        {
            return _variableMap.FirstOrDefault(variableMap => variableMap.ModelVariable.Name.IsEqualTo(variableName));
        }
    }

    internal sealed class SingletonVariableMap
    {
        internal SingletonVariableMap(SingletonVariableModel modelVariable, IntVar solverVariable)
        {
            Contract.Requires<ArgumentNullException>(modelVariable != null);
            SolverVariable = solverVariable;
            ModelVariable = modelVariable;
        }

        internal IntVar SolverVariable { get; }
        internal SingletonVariableModel ModelVariable { get; }
    }
}
