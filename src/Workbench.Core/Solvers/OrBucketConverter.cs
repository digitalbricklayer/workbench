using System;
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
            foreach (var bucket in model.Buckets)
            {
                bucket.PopulateInstances(null);
                var bucketMap = new OrBucketVariableMap(bucket, null);
                var bucketTracker = new OrBucketTracker(bucket, bucketMap);
                ConvertBucket(bucketTracker);
            }
        }

        private void ConvertBucket(OrBucketTracker bucketTracker)
        {
            var bucket = bucketTracker.Bucket;
            var bucketMap = bucketTracker.BucketMap;

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

#if false
                foreach (var innerBucket in bucket.Bundle.Buckets)
                {
                    var innerBucketMap = new OrBucketVariableMap(innerBucket, bucketMap);
                    var innerBucketTracker = new OrBucketTracker(bucket, innerBucketMap);
//                    ConvertBucket(innerBucketTracker);
                }
#endif

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
