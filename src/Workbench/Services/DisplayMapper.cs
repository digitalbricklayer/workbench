using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    internal class DisplayMapper
    {
        /// <summary>
        /// Map a display model to a solution designer view model.
        /// </summary>
        /// <param name="theDisplay">Display model.</param>
        /// <returns>Solution designer view model.</returns>
        public SolutionDesignerViewModel MapFrom(DisplayModel theDisplay)
        {
            return new SolutionDesignerViewModel(theDisplay);
        }
    }
}