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
        private readonly IViewModelService cache;
        private readonly IEventAggregator eventAggregator;

        public VariableMapper(IViewModelService theService, IEventAggregator theEventAggregator)
        {
            this.cache = theService;
            this.eventAggregator = theEventAggregator;
        }

        internal VariableViewModel MapFrom(VariableGraphicModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new VariableViewModel(theVariableModel,
                                                          this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }

        internal AggregateVariableViewModel MapFrom(AggregateVariableGraphicModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new AggregateVariableViewModel(theVariableModel,
                                                                   this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }
    }
}