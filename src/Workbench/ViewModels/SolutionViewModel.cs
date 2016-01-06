using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution.
    /// </summary>
    public sealed class SolutionViewModel : Conductor<GraphicViewModel>.Collection.AllActive
    {
        /// <summary>
        /// Initialize the solution with a solution model.
        /// </summary>
        /// <param name="theSolution">The solution model.</param>
        public SolutionViewModel(SolutionModel theSolution)
        {
            if (theSolution == null)
                throw new ArgumentNullException("theSolution");
            this.Values = new ObservableCollection<ValueViewModel>();
            this.Model = new SolutionModel();
        }

        /// <summary>
        /// Initialize the solution with default values.
        /// </summary>
        public SolutionViewModel()
        {
            this.Values = new ObservableCollection<ValueViewModel>();
            this.Model = new SolutionModel();
        }

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public ObservableCollection<ValueViewModel> Values
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Model { get; set; }

        /// <summary>
        /// Bind the values to the solution.
        /// </summary>
        /// <param name="theValues">Values.</param>
        public void BindTo(IEnumerable<ValueViewModel> theValues)
        {
            this.Reset();
            foreach (var value in theValues)
                this.Values.Add(value);
        }

        /// <summary>
        /// Reset the contents of the solution.
        /// </summary>
        public void Reset()
        {
            this.Values.Clear();
        }

        /// <summary>
        /// Add a value.
        /// </summary>
        /// <param name="newValueViewModel">New value.</param>
        public void AddValue(ValueViewModel newValueViewModel)
        {
            if (newValueViewModel == null)
                throw new ArgumentNullException("newValueViewModel");
            this.Values.Add(newValueViewModel);
        }
    }
}
