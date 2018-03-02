using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class SingletonVariableViewerViewModel : VariableViewerViewModel
    {
        public SingletonVariableViewerViewModel(SingletonVariableGraphicModel theSingletonVariableModel)
            : base(theSingletonVariableModel)
        {
            Model = theSingletonVariableModel;
        }

        public override bool IsAggregate => false;
    }
}