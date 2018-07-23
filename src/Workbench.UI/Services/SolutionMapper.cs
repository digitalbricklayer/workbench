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
        private readonly IEventAggregator eventAggregator;

        public SolutionMapper(IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            this.eventAggregator = theEventAggregator;
        }

#if false
        /// <summary>
        /// Map a solution model into a view model.
        /// </summary>
        /// <param name="theSolutionModel">Solution model.</param>
        /// <returns>Solution view model.</returns>
        public WorkspaceViewerViewModel MapFrom(SolutionModel theSolutionModel)
        {
            var solutionViewModel = new WorkspaceViewerViewModel(theSolutionModel);
            foreach (var valueModel in theSolutionModel.Snapshot.SingletonValues)
            {
                solutionViewModel.AddValue(valueModel);
            }

            foreach (var anAggregateValue in theSolutionModel.Snapshot.AggregateValues)
            {
                solutionViewModel.AddValue(anAggregateValue);
            }
           
            return solutionViewModel;
        }
#endif
    }
}
