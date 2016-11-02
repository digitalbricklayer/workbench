using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// All different constraint expression model contains which variable is to 
    /// be constrained by the all different constraint.
    /// </summary>
    [Serializable]
    public class AllDifferentConstraintExpressionModel : AbstractModel
    {
        private string text;

        public AllDifferentConstraintExpressionModel(string rawExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(rawExpression));
            Text = rawExpression;
        }

        public AllDifferentConstraintExpressionModel()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the text of the all different constraint expression.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                OnPropertyChanged();
            }
        }
    }
}
