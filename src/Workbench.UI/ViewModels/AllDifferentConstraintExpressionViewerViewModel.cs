using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the all different constraint expression viewer.
    /// </summary>
    public sealed class AllDifferentConstraintExpressionViewerViewModel : PropertyChangedBase
    {
        private string text;

        public AllDifferentConstraintExpressionViewerViewModel(AllDifferentConstraintExpressionModel theExpressionModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
            Model = theExpressionModel;
        }

        /// <summary>
        /// Gets or sets the all different constraint expression model.
        /// </summary>
        public AllDifferentConstraintExpressionModel Model { get; set; }

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
