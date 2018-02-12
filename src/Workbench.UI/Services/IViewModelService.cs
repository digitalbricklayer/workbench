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
        void CacheVariable(VariableGraphicViewModel variableViewModel);
        void CacheGraphic(GraphicViewModel graphicViewModel);
        GraphicViewModel GetGraphicByIdentity(int graphicIdentity);

        /// <summary>
        /// Get the variable matching the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable matching the identity.</returns>
        VariableGraphicViewModel GetVariableByIdentity(int variableIdentity);

        /// <summary>
        /// Get all variable view models.
        /// </summary>
        /// <returns>All variable view models in the model.</returns>
        IReadOnlyCollection<VariableGraphicViewModel> GetAllVariables();
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

        public void CacheVariable(VariableGraphicViewModel variableViewModel)
        {
            Contract.Requires<ArgumentNullException>(variableViewModel != null);
        }

        public void CacheGraphic(GraphicViewModel graphicViewModel)
        {
            Contract.Requires<ArgumentNullException>(graphicViewModel != null);
        }

        public GraphicViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            Contract.Requires<ArgumentException>(graphicIdentity != default(int));
            return default(GraphicViewModel);
        }

        public VariableGraphicViewModel GetVariableByIdentity(int variableIdentity)
        {
            Contract.Requires<ArgumentException>(variableIdentity != default(int));
            return default(VariableGraphicViewModel);
        }

        public IReadOnlyCollection<VariableGraphicViewModel> GetAllVariables()
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<VariableGraphicViewModel>>() != null);
            return default(IReadOnlyCollection<VariableGraphicViewModel>);
        }
    }
}