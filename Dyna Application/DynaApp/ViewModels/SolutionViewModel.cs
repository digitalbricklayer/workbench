using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynaApp.Entities;
using DynaApp.Models;

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
        public SolutionViewModel(IEnumerable<ValueViewModel> theValues)
        {
            if (theValues == null)
                throw new ArgumentNullException("theValues");
            this.Values = new ObservableCollection<ValueViewModel>(theValues);
        }

        /// <summary>
        /// Initialize the solution with default values.
        /// </summary>
        public SolutionViewModel()
        {
            this.Values = new ObservableCollection<ValueViewModel>();
        }

        /// <summary>
        /// Gets the values displayed in the solution.
        /// </summary>
        public ObservableCollection<ValueViewModel> Values
        {
            get; private set;
        }

        /// <summary>
        /// Bind the bound values to the solution.
        /// </summary>
        /// <param name="theValues">Bound values.</param>
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

        public static SolutionViewModel For(SolutionModel solution)
        {
            return new SolutionViewModel
            {
                Values = new ObservableCollection<ValueViewModel>(ValueViewModel.For(solution.Values))
            };
        }
    }
}
