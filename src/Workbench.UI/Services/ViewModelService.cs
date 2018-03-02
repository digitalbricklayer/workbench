using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Service responsible for cross cutting concerns across all view models.
    /// </summary>
    public sealed class ViewModelService : IViewModelService, IDisposable
    {
        private readonly Dictionary<int, VisualizerViewModel> graphicMap;
        private readonly Dictionary<int, VariableVisualizerViewModel> variableMap;
		private readonly IViewModelFactory viewModelFactory;

        /// <summary>
        /// Initialize the view model cache with default values.
        /// </summary>
        public ViewModelService(IViewModelFactory theViewModelFactory)
        {
			Contract.Requires<ArgumentNullException>(theViewModelFactory != null);
			this.viewModelFactory = theViewModelFactory;
			this.viewModelFactory.WorkAreaCreated += OnWorkAreaCreated;
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
		
		public void Dispose()
		{
			this.viewModelFactory.WorkAreaCreated -= OnWorkAreaCreated;
		}
		
		private void OnWorkAreaCreated(Object sender, WorkAreaCreatedArgs e)
		{
		}
    }
}
