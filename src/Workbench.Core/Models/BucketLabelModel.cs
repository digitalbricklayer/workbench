using System;
using System.Collections.Generic;

namespace Workbench.Core.Models
{
    public sealed class BucketLabelModel
    {
        public BucketLabelModel(BucketVariableModel bucket, IReadOnlyCollection<BundleLabelModel> bundleLabels)
        {
            Bucket = bucket;
            BundleLabels = bundleLabels;
        }

        public IReadOnlyCollection<BundleLabelModel> BundleLabels { get; }

        public BucketVariableModel Bucket { get; }
    }
}