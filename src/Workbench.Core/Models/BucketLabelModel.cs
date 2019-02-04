using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    public sealed class BucketLabelModel
    {
        public BucketLabelModel(BucketModel bucket, IReadOnlyCollection<ValueModel> values)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Contract.Requires<ArgumentNullException>(values != null);
            Bucket = bucket;
            Values = values;
        }

        public IReadOnlyCollection<ValueModel> Values { get; }

        public BucketModel Bucket { get; }
    }
}