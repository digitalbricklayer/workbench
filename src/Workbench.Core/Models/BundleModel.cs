using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A bundle model is a composable repository of sub-bundles, constraints, variables and shared domains.
    /// </summary>
    [Serializable]
    public sealed class BundleModel : Model
    {
        public BundleModel(BundleModel parent, int count, BundleTemplateModel bundle, string name)
        {
            ParentInstance = parent;
            Count = count;
            Template = bundle;
            Name = name;
        }

        public string Name { get; internal set; }

        public BundleTemplateModel Template { get; }

        public int Count { get; }

        public BundleModel ParentInstance { get; }
    }
}
