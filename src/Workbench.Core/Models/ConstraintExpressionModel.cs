using System;
using System.Diagnostics.Contracts;
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

        /// <summary>
        /// Constraint expression AST tree.
        /// </summary>
        [NonSerialized]
        private ConstraintExpressionNode node;

        public ConstraintExpressionModel(string rawExpression)
        {
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            Text = rawExpression;
        }

        public ConstraintExpressionModel()
        {
            Text = string.Empty;
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
                ParseUnit(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the constraint expression root node.
        /// </summary>
        public ConstraintExpressionNode Node
        {
            get { return node; }
            private set { node = value; }
        }

        /// <summary>
        /// Gets the operator type of the expression.
        /// </summary>
        public OperatorType OperatorType => Node.InnerExpression.Operator;

        /// <summary>
        /// Returns a string that represents the constraint expression.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{Node.InnerExpression.LeftExpression} {OperatorType} {Node.InnerExpression.RightExpression}";
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
                if (parseResult.Status == ParseStatus.Success)
                    Node = parseResult.Root;
            }
            else
            {
                Node = null;
            }
        }
    }
}
