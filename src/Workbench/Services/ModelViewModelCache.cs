using System.Collections.Generic;
using System.Diagnostics;
using Workbench.ViewModels;

namespace Workbench.Services
{
    public class ModelViewModelCache
    {
        private readonly Dictionary<int, GraphicViewModel> graphicMap;
        private readonly Dictionary<int, VariableViewModel> variableMap;

        public ModelViewModelCache()
        {
            this.graphicMap = new Dictionary<int, GraphicViewModel>();
            this.variableMap = new Dictionary<int, VariableViewModel>();
        }

        public void CacheVariable(VariableViewModel variableViewModel)
        {
            Debug.Assert(variableViewModel.Id != default(int));
            this.CacheGraphic(variableViewModel);
            this.variableMap.Add(variableViewModel.Id, variableViewModel);
        }

        public void CacheGraphic(GraphicViewModel graphicViewModel)
        {
            Debug.Assert(graphicViewModel.Id != default(int));
            this.graphicMap.Add(graphicViewModel.Id, graphicViewModel);
        }

        public GraphicViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            Debug.Assert(graphicIdentity != default(int));
            return this.graphicMap[graphicIdentity];
        }

        public VariableViewModel GetVariableByIdentity(int variableIdentity)
        {
            Debug.Assert(variableIdentity != default(int));
            return this.variableMap[variableIdentity];
        }
    }
}
