using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a variable model into a view model.
    /// </summary>
    public sealed class VariableMapper
    {
        private readonly IViewModelService cache;
        private readonly IEventAggregator eventAggregator;

        public VariableMapper(IViewModelService theService, IEventAggregator theEventAggregator)
        {
            this.cache = theService;
            this.eventAggregator = theEventAggregator;
        }

        public VariableGraphicViewModel MapFrom(SingletonVariableGraphicModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new SingletonVariableViewModel(theVariableModel, this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }

        public AggregateVariableViewModel MapFrom(AggregateVariableGraphicModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new AggregateVariableViewModel(theVariableModel, this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }
    }
}