using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a variable model into a view model.
    /// </summary>
    public class VariableMapper
    {
        private readonly IViewModelCache cache;
        private readonly IEventAggregator eventAggregator;

        public VariableMapper(IViewModelCache theCache, IEventAggregator theEventAggregator)
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