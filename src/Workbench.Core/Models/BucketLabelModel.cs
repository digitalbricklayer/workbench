using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    public sealed class BucketLabelModel
    {
        public BucketLabelModel(BucketVariableModel bucket, IReadOnlyCollection<BundleLabelModel> bundleLabels)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Contract.Requires<ArgumentNullException>(bundleLabels != null);
            Bucket = bucket;
            BundleLabels = bundleLabels;
        }

        public IReadOnlyCollection<BundleLabelModel> BundleLabels { get; }

        public BucketVariableModel Bucket { get; }
    }
}