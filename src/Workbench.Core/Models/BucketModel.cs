using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A bucket is a container for bundles.
    /// </summary>
    public class BucketModel : Model
    {
        /// <summary>
        /// Initialize a bucket with a name, size and bundle name.
        /// </summary>
        /// <param name="name">Name of the bucket.</param>
        /// <param name="size">Size of the bucket.</param>
        /// <param name="bundleName">Name of the bundle contained within the bucket.</param>
        public BucketModel(ModelName name, int size, string bundleName)
            : base(name)
        {
            Contract.Requires<ArgumentOutOfRangeException>(size > 0);
            Size = size;
            BundleName = bundleName;
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
        /// Gets the bundle name contained inside the bucket.
        /// </summary>
        public string BundleName { get; }

        /// <summary>
        /// Gets or sets the size of the bucket.
        /// </summary>
        public int Size { get; }
    }
}
