using System;
using System.Collections.Generic;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A bucket variable is a container for bundles.
    /// </summary>
    public sealed class BucketVariableModel : Model
    {
        /// <summary>
        /// Initialize a bucket with a name, size and bundle name.
        /// </summary>
        /// <param name="workspace">Workspace the variable resides.</param>
        /// <param name="name">Name of the bucket.</param>
        /// <param name="size">Size of the bucket.</param>
        /// <param name="bundle">Bundle contained within the bucket.</param>
        public BucketVariableModel(WorkspaceModel workspace, ModelName name, int size, BundleModel bundle)
            : base(name)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            Size = size;
            Bundle = bundle;
            Instances = new List<BundleInstanceModel>();
        }

        /// <summary>
        /// Gets the bundle contained inside the bucket.
        /// </summary>
        public BundleModel Bundle { get; }

        /// <summary>
        /// Gets the bundle instances.
        /// </summary>
        public IList<BundleInstanceModel> Instances { get; private set; }

        /// <summary>
        /// Gets or sets the size of the bucket.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Get the size of the bucket variable.
        /// </summary>
        /// <returns>Size of the bucket variable.</returns>
        public long GetSize()
        {
            return Size;
        }

        internal void PopulateInstances(BundleInstanceModel parent)
        {
            var instances = new List<BundleInstanceModel>();
            for (var i = 0; i < Size; i++)
            {
                var instance = CreateInstanceFrom(Bundle, i);
                Bundle.Instance = instance;
                var instanceName = CreateNameFrom(Bundle, i);
                Bundle.Instance.Name = instanceName;
                instances.Add(instance);
            }

            Instances = instances;

            foreach (var bucket in Bundle.Buckets)
            {
                bucket.PopulateInstances(Bundle.Instance);
            }
        }

        private string CreateNameFrom(BundleModel bundle, int i)
        {
            return bundle.Instance.Name + bundle.Name + "_" + i;
        }

        private BundleInstanceModel CreateInstanceFrom(BundleModel bundle, int count)
        {
            return new BundleInstanceModel(Bundle.Instance, count, bundle);
        }
    }
}
