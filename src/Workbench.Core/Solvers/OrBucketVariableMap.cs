using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class OrBucketVariableMap
    {
        private readonly List<OrBundleMap> _bundleMaps;

        internal OrBucketVariableMap(BucketVariableModel bucket)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Bucket = bucket;
            _bundleMaps = new List<OrBundleMap>();
        }

        internal BucketVariableModel Bucket { get; }

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