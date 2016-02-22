using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Map the solution model into a view model.
    /// </summary>
    public class SolutionMapper
    {
        private readonly ValueMapper valueMapper;
        private readonly IViewModelService _viewModelService;
        private readonly IEventAggregator eventAggregator;

        public SolutionMapper(IViewModelService theService, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            this._viewModelService = theService;
            this.eventAggregator = theEventAggregator;
            this.valueMapper = new ValueMapper(theService);
        }

        /// <summary>
        /// Map a solution model into a view model.
        /// </summary>
        /// <param name="theSolutionModel">Solution model.</param>
        /// <returns>Solution view model.</returns>
        internal SolutionViewerViewModel MapFrom(SolutionModel theSolutionModel)
        {
            var solutionViewModel = new SolutionViewerViewModel(theSolutionModel);
            foreach (var valueModel in theSolutionModel.SingletonValues)
            {
                solutionViewModel.AddValue(this.valueMapper.MapFrom(valueModel));
            }

            foreach (var aVisualizer in theSolutionModel.Display.Visualizers)
            {
                var newViewer = new VariableVisualizerViewerViewModel(aVisualizer,
                                                                      this.eventAggregator);
                if (aVisualizer.Binding != null)
                    newViewer.Binding = this._viewModelService.GetVariableByIdentity(aVisualizer.Binding.VariableId);
                solutionViewModel.ActivateItem(newViewer);
            }

            return solutionViewModel;
        }
    }
}