using System;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Constraint expression model.
    /// </summary>
    [Serializable]
    public class ConstraintExpressionModel : AbstractModel
    {
        private string text;

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
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the constraint expression root node.
        /// </summary>
        public ConstraintExpressionNode Node { get; private set; }

        /// <summary>
        /// Gets the operator type of the expression.
        /// </summary>
        public OperatorType OperatorType
        {
            get { return this.Node.InnerExpression.Operator; }
        }

        /// <summary>
        /// Returns a string that represents the constraint expression.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Node.InnerExpression.LeftExpression, OperatorType, Node.InnerExpression.RightExpression);
        }

        /// <summary>
        /// Parse the raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
            {
                var interpreter = new ConstraintExpressionParser();
                var parseResult = interpreter.Parse(rawExpression);
                if (parseResult.Status == ConstraintExpressionParseStatus.Success)
                    this.Node = parseResult.Root;
            }
            else
            {
                this.Node = null;
            }
        }
    }
}
