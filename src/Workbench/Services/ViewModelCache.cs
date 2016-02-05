using System;
using System.Collections.Generic;
using System.Diagnostics;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Cache view models used in the workspace.
    /// </summary>
    public class ViewModelCache : IViewModelCache
    {
        private readonly Dictionary<int, GraphicViewModel> graphicMap;
        private readonly Dictionary<int, VariableViewModel> variableMap;

        /// <summary>
        /// Initialize the view model cache with default values.
        /// </summary>
        public ViewModelCache()
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

        /// <summary>
        /// Get the variable with the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable with the identity.</returns>
        public VariableViewModel GetVariableByIdentity(int variableIdentity)
        {
            Debug.Assert(variableIdentity != default(int));
            return this.variableMap[variableIdentity];
        }
    }
}
