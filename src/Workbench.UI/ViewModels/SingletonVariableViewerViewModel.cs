using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class SingletonVariableViewerViewModel : VariableViewerViewModel
    {
        public SingletonVariableViewerViewModel(SingletonVariableGraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            Model = theGraphicModel;
        }
    }
}