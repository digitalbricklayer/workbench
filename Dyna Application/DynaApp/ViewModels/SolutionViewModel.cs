using System.Collections.Generic;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the solution.
    /// </summary>
    public sealed class SolutionViewModel : AbstractViewModel
    {
        private readonly List<BoundVariableViewModel> values = new List<BoundVariableViewModel>();

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public IEnumerator<BoundVariableViewModel> Values
        {
            get
            {
                return this.values.GetEnumerator();
            }
        }
    }
}
