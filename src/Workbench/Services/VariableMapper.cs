using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a variable model into a view model.
    /// </summary>
    internal class VariableMapper
    {
        private readonly ViewModelCache cache;
        private readonly IEventAggregator eventAggregator;

        internal VariableMapper(ViewModelCache theCache, IEventAggregator theEventAggregator)
        {
            this.cache = theCache;
            this.eventAggregator = theEventAggregator;
        }

        internal VariableViewModel MapFrom(VariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new VariableViewModel(theVariableModel,
                                                          this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }

        internal AggregateVariableViewModel MapFrom(AggregateVariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new AggregateVariableViewModel(theVariableModel,
                                                                   this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }
    }
}