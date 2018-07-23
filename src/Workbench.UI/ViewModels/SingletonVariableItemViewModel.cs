using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class SingletonVariableItemViewModel : VariableItemViewModel
    {
        public SingletonVariableItemViewModel(SingletonVariableModel theSingletonVariableModel)
            : base(theSingletonVariableModel)
        {
            SingletonVariable = theSingletonVariableModel;
        }

        public SingletonVariableModel SingletonVariable { get; set; }
    }
}