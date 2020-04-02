using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public sealed class BundleInstanceModel : Model
    {
        public BundleInstanceModel(BundleInstanceModel parent, int count, BundleModel bundle)
        {
            ParentInstance = parent;
            Count = count;
            Bundle = bundle;
            Name = string.Empty;
        }

        public string Name { get; internal set; }

        public BundleModel Bundle { get; }

        public int Count { get; }

        public BundleInstanceModel ParentInstance { get; }
    }
}
