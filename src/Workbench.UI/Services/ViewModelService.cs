using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.ViewModels;

namespace Workbench.Services
{
#if false
    /// <summary>
    /// Service responsible for cross cutting concerns across all view models.
    /// </summary>
    public sealed class ViewModelService : IViewModelService
    {
        private readonly Dictionary<int, VisualizerViewModel> graphicMap;
        private readonly Dictionary<int, VariableVisualizerViewModel> variableMap;

        /// <summary>
        /// Initialize the view model cache with default values.
        /// </summary>
        public ViewModelService()
        {
            this.graphicMap = new Dictionary<int, VisualizerViewModel>();
            this.variableMap = new Dictionary<int, VariableVisualizerViewModel>();
        }

        public void CacheVariable(VariableVisualizerViewModel variableViewModel)
        {
            this.CacheGraphic(variableViewModel);
            this.variableMap.Add(variableViewModel.Id, variableViewModel);
        }

        public void CacheGraphic(VisualizerViewModel graphicViewModel)
        {
            this.graphicMap.Add(graphicViewModel.Id, graphicViewModel);
        }

        public VisualizerViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            return this.graphicMap[graphicIdentity];
        }

        /// <summary>
        /// Get the variable with the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable with the identity.</returns>
        public VariableVisualizerViewModel GetVariableByIdentity(int variableIdentity)
        {
            return this.variableMap[variableIdentity];
        }

        /// <summary>
        /// Get all variable view models.
        /// </summary>
        /// <returns>All variable view models in the model.</returns>
        public IReadOnlyCollection<VariableVisualizerViewModel> GetAllVariables()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<VariableVisualizerViewModel>>() != null);
            return this.variableMap.Values.ToList();
        }
    }
#endif
}
