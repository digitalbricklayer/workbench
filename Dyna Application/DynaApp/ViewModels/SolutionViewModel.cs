using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for the solution.
    /// </summary>
    public sealed class SolutionViewModel : AbstractViewModel
    {
        /// <summary>
        /// Initialize the solution with bound values.
        /// </summary>
        /// <param name="theValues">Bound values.</param>
        public SolutionViewModel(IEnumerable<BoundVariableViewModel> theValues)
        {
            if (theValues == null)
                throw new ArgumentNullException("theValues");
            this.Values = new ObservableCollection<BoundVariableViewModel>(theValues);
        }

        /// <summary>
        /// Initialize the solution with default values.
        /// </summary>
        public SolutionViewModel()
        {
            this.Values = new ObservableCollection<BoundVariableViewModel>();
        }

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public ObservableCollection<BoundVariableViewModel> Values
        {
            get; private set;
        }

        /// <summary>
        /// Bind the bound values to the solution.
        /// </summary>
        /// <param name="theValues">Bound values.</param>
        public void BindTo(IEnumerable<BoundVariableViewModel> theValues)
        {
            this.Values.Clear();
            foreach (var value in theValues)
                this.Values.Add(value);
        }
    }
}
