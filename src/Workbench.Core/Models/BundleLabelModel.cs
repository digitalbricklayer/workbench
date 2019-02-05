using System.Collections.Generic;
using System.Linq;

namespace Workbench.Core.Models
{
    public sealed class BundleLabelModel
    {
        public BundleLabelModel(BundleModel bundle, IReadOnlyCollection<SingletonLabelModel> labels)
        {
            Bundle = bundle;
            Labels = labels;
        }

        public BundleModel Bundle { get; }

        public IReadOnlyCollection<SingletonLabelModel> Labels { get; }

        public SingletonLabelModel GetSingletonLabelByName(string variableName)
        {
            return Labels.FirstOrDefault(label => label.SingletonVariable.Name == variableName);
        }
    }
}