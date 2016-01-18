using System.Diagnostics;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a variable model into a view model.
    /// </summary>
    internal class VariableMapper
    {
        private readonly ModelViewModelCache cache;

        internal VariableMapper(ModelViewModelCache theCache)
        {
            this.cache = theCache;
        }

        internal VariableViewModel MapFrom(VariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new VariableViewModel(theVariableModel);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }

        internal AggregateVariableViewModel MapFrom(AggregateVariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new AggregateVariableViewModel(theVariableModel);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }
    }
}