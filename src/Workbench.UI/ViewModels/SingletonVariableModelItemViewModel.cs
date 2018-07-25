using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class SingletonVariableModelItemViewModel : VariableModelItemViewModel
    {
        public SingletonVariableModelItemViewModel(SingletonVariableModel theSingletonVariableModel)
            : base(theSingletonVariableModel)
        {
            SingletonVariable = theSingletonVariableModel;
        }

        public SingletonVariableModel SingletonVariable { get; set; }
    }
}