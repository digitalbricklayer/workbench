using System.Collections.Generic;
using Workbench.ViewModels;

namespace Workbench.Services
{
    public interface IViewModelCache
    {
        void CacheVariable(VariableViewModel variableViewModel);
        void CacheGraphic(GraphicViewModel graphicViewModel);
        GraphicViewModel GetGraphicByIdentity(int graphicIdentity);

        /// <summary>
        /// Get the variable matching the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable matching the identity.</returns>
        VariableViewModel GetVariableByIdentity(int variableIdentity);

        /// <summary>
        /// Get all variable view models.
        /// </summary>
        /// <returns>All variable view models in the model.</returns>
        IReadOnlyCollection<VariableViewModel> GetAllVariables();
    }
}