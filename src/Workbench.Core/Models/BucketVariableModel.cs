using System;

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
        }

        /// <summary>
        /// Initialize a bucket with a name.
        /// </summary>
        /// <param name="workspace">Workspace the variable resides.</param>
        /// <param name="name">Bucket name.</param>
        public BucketVariableModel(WorkspaceModel workspace, ModelName name)
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

        /// <summary>
        /// Get the size of the bucket variable.
        /// </summary>
        /// <returns>Size of the bucket variable.</returns>
        public long GetSize()
        {
            return Size;
        }
    }
}