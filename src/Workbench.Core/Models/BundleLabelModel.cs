using System.Collections.Generic;
using System.Linq;

namespace Workbench.Core.Models
{
    public sealed class BundleLabelModel
    {
        public BundleLabelModel(BundleModel bundle, IReadOnlyCollection<SingletonVariableLabelModel> labels)
        {
            Bundle = bundle;
            Labels = labels;
        }

        public BundleModel Bundle { get; }

        public IReadOnlyCollection<SingletonVariableLabelModel> Labels { get; }

        public SingletonVariableLabelModel GetSingletonLabelByName(string variableName)
        {
            return Labels.FirstOrDefault(label => label.SingletonVariable.Name == variableName);
        }
    }
}