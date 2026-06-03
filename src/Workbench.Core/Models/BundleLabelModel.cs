using System.Collections.Generic;
using System.Linq;

namespace Workbench.Core.Models
{
    public sealed class BundleLabelModel
    {
        public BundleLabelModel(BundleTemplateModel bundle, IReadOnlyCollection<SingletonVariableLabelModel> labels)
        {
            Bundle = bundle;
            Labels = labels;
        }

        public BundleTemplateModel Bundle { get; }

        public IReadOnlyCollection<SingletonVariableLabelModel> Labels { get; }

        public SingletonVariableLabelModel GetSingletonLabelByName(string variableName)
        {
            return Labels.FirstOrDefault(label => label.SingletonVariable.Name == variableName);
        }
    }
}