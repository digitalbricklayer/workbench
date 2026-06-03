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
        /// <param name="bundleTemplate">Bundle template.</param>
        public BucketVariableModel(WorkspaceModel workspace, ModelName name, int size, BundleTemplateModel bundleTemplate)
            : base(name)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            Size = size;
            Template = bundleTemplate;
            Bundles = new List<BundleModel>();
        }

        /// <summary>
        /// Gets the bundle contained inside the bucket.
        /// </summary>
        public BundleTemplateModel Template { get; }

        /// <summary>
        /// Gets the bundle instances.
        /// </summary>
        public IList<BundleModel> Bundles { get; private set; }

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

        internal void PopulateInstances(BundleModel parent)
        {
            var bundles = new List<BundleModel>();
            for (var i = 0; i < Size; i++)
            {
                var instance = CreateBundleFrom(Template, i);
                Template.Instance = instance;
                bundles.Add(instance);
            }

            Bundles = bundles;

            foreach (var bucket in Template.Buckets)
            {
                bucket.PopulateInstances(Template.Instance);
            }
        }

        private string CreateNameFrom(BundleTemplateModel bundle, int i)
        {
            return bundle.Instance.Name + bundle.Name + "_" + i;
        }

        private BundleModel CreateBundleFrom(BundleTemplateModel bundleTemplate, int count)
        {
            return new BundleModel(Template.Instance, count, bundleTemplate, CreateNameFrom(Template, count));
        }
    }
}
