using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ConstraintViewerViewModel : ViewerViewModel
    {
        protected ConstraintViewerViewModel(GraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
        }
    }
}