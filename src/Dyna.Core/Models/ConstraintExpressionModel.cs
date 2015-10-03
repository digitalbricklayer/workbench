using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Constraint expression model.
    /// </summary>
    [Serializable]
    public class ConstraintExpressionModel
    {
        private string text;

        public ConstraintExpressionModel(ConstraintExpressionUnit theExpressionUnit)
        {
            if (theExpressionUnit == null)
                throw new ArgumentNullException("theExpressionUnit");
            this.Unit = theExpressionUnit;
        }

        public ConstraintExpressionModel(string rawExpression)
        {
            this.Text = rawExpression;
        }

        public ConstraintExpressionModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the expression as text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.ParseUnit(value);
            }
        }

        /// <summary>
        /// Gets the expression unit.
        /// </summary>
        public ConstraintExpressionUnit Unit { get; private set; }

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public VariableModel Left
        {
            get { return this.Unit.Left; }
        }

        /// <summary>
        /// Gets the right hand side expression.
        /// </summary>
        public Expression Right
        {
            get { return this.Unit.Right; }
        }

        /// <summary>
        /// Gets the operator type of the expression.
        /// </summary>
        public OperatorType OperatorType
        {
            get { return this.Unit.OperatorType; }
        }

        /// <summary>
        /// Returns a string that represents the constraint expression.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Left, OperatorType, Right);
        }

        /// <summary>
        /// Parse the raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
                this.Unit = ConstraintGrammar.Parse(rawExpression);
            else
                this.Unit = null;
        }
    }
}