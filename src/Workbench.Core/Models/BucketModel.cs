using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A bucket is a container for bundles.
    /// </summary>
    public sealed class BucketModel : Model
    {
        /// <summary>
        /// Initialize a bucket with a name, size and bundle name.
        /// </summary>
        /// <param name="name">Name of the bucket.</param>
        /// <param name="size">Size of the bucket.</param>
        /// <param name="bundle">Bundle contained within the bucket.</param>
        public BucketModel(ModelName name, int size, BundleModel bundle)
            : base(name)
        {
            Contract.Requires<ArgumentOutOfRangeException>(size > 0);
            Contract.Requires<ArgumentNullException>(bundle != null);
            Size = size;
            Bundle = bundle;
        }

        /// <summary>
        /// Initialize a bucket with a name.
        /// </summary>
        /// <param name="name">Bucket name.</param>
        public BucketModel(ModelName name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets the bundle contained inside the bucket.
        /// </summary>
        public BundleModel Bundle { get; }

        /// <summary>
        /// Gets or sets the size of the bucket.
        /// </summary>
        public int Size { get; }
    }
}