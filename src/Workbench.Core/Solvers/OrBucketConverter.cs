using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Convert the buckets into a representation understood by or-tools.
    /// </summary>
    internal class OrBucketConverter
    {
        private readonly Google.OrTools.ConstraintSolver.Solver _solver;
        private readonly OrToolsCache _cache;
        private readonly OrValueMapper _valueMapper;

        /// <summary>
        /// Initialize the bucket converter with a Google or-tools solver, a or-tools cache and a solver / domain mapper.
        /// </summary>
        internal OrBucketConverter(Google.OrTools.ConstraintSolver.Solver solver, OrToolsCache cache, OrValueMapper valueMapper)
        {
            Contract.Requires<ArgumentNullException>(solver != null);
            Contract.Requires<ArgumentNullException>(cache != null);
            Contract.Requires<ArgumentNullException>(valueMapper != null);

            _solver = solver;
            _cache = cache;
            _valueMapper = valueMapper;
        }

        /// <summary>
        /// Convert all buckets into a representation understood by the solver.
        /// </summary>
        /// <param name="model">The model.</param>
        internal void ConvertBuckets(ModelModel model)
        {
            Contract.Requires<ArgumentNullException>(model != null);

            foreach (var aBucket in model.Buckets)
            {
                ConvertBucket(aBucket);
            }
        }

        private void ConvertBucket(BucketVariableModel bucket)
        {
            var bucketMap = new OrBucketVariableMap(bucket);
            for (var i = 0; i < bucket.Size; i++)
            {
                var bundleMap = new OrBundleMap(bucket.Bundle);
                foreach (var singleton in bucket.Bundle.Singletons)
                {
                    var variableBand = VariableBandEvaluator.GetVariableBand(singleton);
                    _valueMapper.AddBucketDomainValue(bucket, variableBand);
                    var variableRange = variableBand.GetRange();
                    var orToolsVariableName = CreateOrToolsVariableNameFrom(bucket, i, singleton);
                    var orVariable = _solver.MakeIntVar(variableRange.Lower, variableRange.Upper, orToolsVariableName);
                    bundleMap.Add(singleton, orVariable);
                    _cache.AddVariable(orVariable);
                }

                bucketMap.Add(bundleMap);
            }
            _cache.AddBucket(bucket.Name, bucketMap);
        }

        private static string CreateOrToolsVariableNameFrom(BucketVariableModel bucket, int index, SingletonVariableModel singletonVariable)
        {
            return bucket.Name + "_" + Convert.ToString(index) + "_" + singletonVariable.Name;
        }
    }
}