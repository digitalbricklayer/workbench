using System;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Map the solution model into a view model.
    /// </summary>
    internal class SolutionMapper
    {
        private readonly ValueMapper valueMapper;

        internal SolutionMapper(ModelViewModelCache theCache)
        {
            if (theCache == null)
                throw new ArgumentNullException("theCache");
            this.valueMapper = new ValueMapper(theCache);
        }

        /// <summary>
        /// Map a solution model into a view model.
        /// </summary>
        /// <param name="theSolutionModel">Solution model.</param>
        /// <returns>Solution view model.</returns>
        internal SolutionViewModel MapFrom(SolutionModel theSolutionModel)
        {
            var solutionViewModel = new SolutionViewModel();
            solutionViewModel.Model = theSolutionModel;
            foreach (var valueModel in theSolutionModel.Values)
            {
                solutionViewModel.AddValue(this.valueMapper.MapFrom(valueModel));
            }

            return solutionViewModel;
        }
    }
}