using System.Collections.Generic;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Track the creation of buckets and associated bundles.
    /// </summary>
    internal sealed class OrBucketTracker
    {
        private readonly List<OrBundleMap> _bundles;

        internal OrBucketTracker(BucketVariableModel bucket, OrBucketVariableMap bucketMap)
        {
            _bundles = new List<OrBundleMap>();
            Bucket = bucket;
            BucketMap = bucketMap;
        }

        internal BucketVariableModel Bucket { get; }

        internal OrBucketVariableMap BucketMap { get; }

        internal IReadOnlyCollection<OrBundleMap> Bundles { get; }

        internal void AddBundle(OrBundleMap bundleMap)
        {
            _bundles.Add(bundleMap);
        }
    }
}
