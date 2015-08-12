using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Map the solution model into a view model.
    /// </summary>
    internal class SolutionMapper
    {
        private WorkspaceMapper workspaceMapper;
        private readonly ValueMapper valueMapper;

        public SolutionMapper(WorkspaceMapper theWorkspaceMapper)
        {
            this.workspaceMapper = theWorkspaceMapper;
            this.valueMapper = new ValueMapper();
        }

        /// <summary>
        /// Map a solution model into a view model.
        /// </summary>
        /// <param name="theSolutionModel">Solution model.</param>
        /// <returns>Solution view model.</returns>
        internal SolutionViewModel MapFrom(SolutionModel theSolutionModel)
        {
            var solutionViewModel = new SolutionViewModel();
            foreach (var valueModel in theSolutionModel.Values)
            {
                solutionViewModel.AddValue(this.valueMapper.MapFrom(valueModel));
            }

            return solutionViewModel;
        }
    }
}