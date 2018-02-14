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

        public SingletonVariableEditorViewModel MapFrom(SingletonVariableGraphicModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

#if false
            var variableViewModel = new SingletonVariableViewModel(theVariableModel, this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
#else
            return null;
#endif
        }

        public AggregateVariableEditorViewModel MapFrom(AggregateVariableGraphicModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

#if false
            var variableViewModel = new AggregateVariableViewModel(theVariableModel, this.eventAggregator);

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
#else
            return null;
#endif
        }
    }
}