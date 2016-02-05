using Workbench.ViewModels;

namespace Workbench.Services
{
    public interface IViewModelCache
    {
        void CacheVariable(VariableViewModel variableViewModel);
        void CacheGraphic(GraphicViewModel graphicViewModel);
        GraphicViewModel GetGraphicByIdentity(int graphicIdentity);

        /// <summary>
        /// Get the variable with the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable with the identity.</returns>
        VariableViewModel GetVariableByIdentity(int variableIdentity);
    }
}