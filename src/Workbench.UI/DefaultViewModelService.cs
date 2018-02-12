using System.Collections.Generic;
using Workbench.Services;
using Workbench.ViewModels;

namespace Workbench
{
    internal class DefaultViewModelService : IViewModelService
    {
        public void CacheVariable(VariableGraphicViewModel variableViewModel)
        {
        }

        public void CacheGraphic(GraphicViewModel graphicViewModel)
        {
        }

        public GraphicViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            return null;
        }

        public VariableGraphicViewModel GetVariableByIdentity(int variableIdentity)
        {
            return null;
        }

        public IReadOnlyCollection<VariableGraphicViewModel> GetAllVariables()
        {
            return null;
        }
    }
}