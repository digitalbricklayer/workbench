using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Contract for the view model service.
    /// </summary>
    [ContractClass(typeof(IViewModelServiceContract))]
    public interface IViewModelService
    {
        void CacheVariable(VariableVisualizerViewModel variableViewModel);
        void CacheGraphic(VisualizerViewModel graphicViewModel);
        VisualizerViewModel GetGraphicByIdentity(int graphicIdentity);

        /// <summary>
        /// Get the variable matching the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable matching the identity.</returns>
        VariableVisualizerViewModel GetVariableByIdentity(int variableIdentity);

        /// <summary>
        /// Get all variable view models.
        /// </summary>
        /// <returns>All variable view models in the model.</returns>
        IReadOnlyCollection<VariableVisualizerViewModel> GetAllVariables();
    }

    /// <summary>
    /// Code contract for the IViewModelService interface.
    /// </summary>
    [ContractClassFor(typeof(IViewModelService))]
    public abstract class IViewModelServiceContract : IViewModelService
    {
        private IViewModelServiceContract()
        {
        }

        public void CacheVariable(VariableVisualizerViewModel variableViewModel)
        {
            Contract.Requires<ArgumentNullException>(variableViewModel != null);
        }

        public void CacheGraphic(VisualizerViewModel graphicViewModel)
        {
            Contract.Requires<ArgumentNullException>(graphicViewModel != null);
        }

        public VisualizerViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            Contract.Requires<ArgumentException>(graphicIdentity != default(int));
            return default(VisualizerViewModel);
        }

        public VariableVisualizerViewModel GetVariableByIdentity(int variableIdentity)
        {
            Contract.Requires<ArgumentException>(variableIdentity != default(int));
            return default(VariableVisualizerViewModel);
        }

        public IReadOnlyCollection<VariableVisualizerViewModel> GetAllVariables()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<VariableVisualizerViewModel>>() != null);
            return default(IReadOnlyCollection<VariableVisualizerViewModel>);
        }
    }
}