using System;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;
using Workbench.Core.Repeaters;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Visualizer property updates expression.
    /// </summary>
    [Serializable]
    public class PropertyUpdateExpressionModel : AbstractModel
    {
        private string _text;

        /// <summary>
        /// Property update expression AST tree.
        /// </summary>
        [NonSerialized]
        private PropertyUpdateExpressionNode _node;

        public PropertyUpdateExpressionModel(string rawExpression)
        {
            if (string.IsNullOrWhiteSpace(rawExpression))
                throw new ArgumentException(nameof(rawExpression));

            Text = rawExpression;
        }

        public PropertyUpdateExpressionModel()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the raw visualizer property expression.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                ParseUnit(this._text);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the property update expression root node.
        /// </summary>
        public PropertyUpdateExpressionNode Node
        {
            get => _node;
            private set => _node = value;
        }

        /// <summary>
        /// Gets whether the expression is empty.
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(Text);

        /// <summary>
        /// Execute the visualizer property update expression.
        /// </summary>
        /// <param name="theContext">Context for updating the visualizer property.</param>
        public object ExecuteWith(PropertyUpdateContext theContext)
        {
            if (Node == null) return string.Empty;
            var interpreter = new PropertyUpdateInterpreter(theContext);
            return interpreter.Process(this);
        }

        /// <summary>
        /// Parse the raw visualizer property expression.
        /// </summary>
        /// <param name="rawExpression">Raw property expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
            {
                var parser = new PropertyBindingExpressionParser();
                var parseResult = parser.Parse(rawExpression);
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
