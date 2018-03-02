using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// A constraint expression viewer view model.
    /// </summary>
    public sealed class ConstraintExpressionViewerViewModel : PropertyChangedBase
    {
        private string text;

        /// <summary>
        /// Initialize a constraint expression viewer with an expression model.
        /// </summary>
        /// <param name="theExpressionModel">Constraint expression model.</param>
        public ConstraintExpressionViewerViewModel(ConstraintExpressionModel theExpressionModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
            this.Model = theExpressionModel;
        }

        /// <summary>
        /// Gets or sets the constraint expression model.
        /// </summary>
        public ConstraintExpressionModel Model { get; set; }

        /// <summary>
        /// Gets or sets the constraint expression text.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
