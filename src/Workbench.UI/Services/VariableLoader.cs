using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a variable model into a view model.
    /// </summary>
    public sealed class VariableLoader
    {
        private readonly IWindowManager _windowManager;

        public VariableLoader(IWindowManager theWindowManager)
        {
            _windowManager = theWindowManager;
        }

        public SingletonVariableModelItemViewModel MapFrom(SingletonVariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new SingletonVariableModelItemViewModel(theVariableModel, _windowManager);

            return variableViewModel;
        }

        public AggregateVariableModelItemViewModel MapFrom(AggregateVariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new AggregateVariableModelItemViewModel(theVariableModel, _windowManager);

            return variableViewModel;
        }
    }
}