using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
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
}