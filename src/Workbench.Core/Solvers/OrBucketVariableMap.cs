using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class OrBucketVariableMap
    {
        private readonly List<OrBundleMap> _bundleMaps;

        internal OrBucketVariableMap(BucketVariableModel bucket, OrBucketVariableMap parent)
        {
            _bundleMaps = new List<OrBundleMap>();
            Bucket = bucket;
            Parent = parent;
        }

        internal BucketVariableModel Bucket { get; }

        internal OrBucketVariableMap Parent { get; }

        internal IReadOnlyCollection<OrBundleMap> GetBundleMaps()
        {
            return new ReadOnlyCollection<OrBundleMap>(_bundleMaps);
        }

        internal void Add(OrBundleMap bundleMap)
        {
            _bundleMaps.Add(bundleMap);
        }

        internal OrBundleMap GetBundleVariableAt(int index)
        {
            return _bundleMaps.ElementAt(index);
        }
    }
}
