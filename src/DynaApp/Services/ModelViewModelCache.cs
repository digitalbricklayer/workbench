using System.Collections.Generic;
using System.Diagnostics;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ModelViewModelCache
    {
        private readonly Dictionary<int, GraphicViewModel> graphicMap;
        private readonly Dictionary<int, VariableViewModel> variableMap;

        internal ModelViewModelCache()
        {
            this.graphicMap = new Dictionary<int, GraphicViewModel>();
            this.variableMap = new Dictionary<int, VariableViewModel>();
        }

        internal void CacheVariable(VariableViewModel variableViewModel)
        {
            Debug.Assert(variableViewModel.Id != default(int));
            this.CacheGraphic(variableViewModel);
            this.variableMap.Add(variableViewModel.Id, variableViewModel);
        }

        internal void CacheGraphic(GraphicViewModel graphicViewModel)
        {
            Debug.Assert(graphicViewModel.Id != default(int));
            this.graphicMap.Add(graphicViewModel.Id, graphicViewModel);
        }

        internal GraphicViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            Debug.Assert(graphicIdentity != default(int));
            return this.graphicMap[graphicIdentity];
        }

        internal VariableViewModel GetVariableByIdentity(int variableIdentity)
        {
            Debug.Assert(variableIdentity != default(int));
            return this.variableMap[variableIdentity];
        }
    }
}
