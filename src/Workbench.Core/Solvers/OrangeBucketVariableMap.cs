using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class OrangeBucketVariableMap
    {
        private readonly List<OrangeBundleMap> _bundleMaps;

        internal OrangeBucketVariableMap(BucketVariableModel bucket)
        {
            Bucket = bucket;
            _bundleMaps = new List<OrangeBundleMap>();
        }

        internal BucketVariableModel Bucket { get; }

        internal IReadOnlyCollection<OrangeBundleMap> GetBundleMaps()
        {
            return new ReadOnlyCollection<OrangeBundleMap>(_bundleMaps);
        }

        internal void Add(OrangeBundleMap bundleMap)
        {
            _bundleMaps.Add(bundleMap);
        }

        internal OrangeBundleMap GetBundleVariableAt(int index)
        {
            return _bundleMaps.ElementAt(index);
        }
    }
}
