using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// All different constraint displayed in a list.
    /// </summary>
    public sealed class AllDifferentConstraintItemViewModel : ConstraintItemViewModel
    {
        public AllDifferentConstraintItemViewModel(AllDifferentConstraintModel theAllDifferentModel)
            : base(theAllDifferentModel)
        {
            Contract.Requires<ArgumentNullException>(theAllDifferentModel != null);
            AllDifferentConstraint = theAllDifferentModel;
            DisplayName = AllDifferentConstraint.Name;
            ExpressionText = AllDifferentConstraint.Expression.Text;
        }

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public AllDifferentConstraintModel AllDifferentConstraint { get; }
    }
}