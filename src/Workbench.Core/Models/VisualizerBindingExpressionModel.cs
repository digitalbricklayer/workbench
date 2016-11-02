using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Nodes;
using Workbench.Core.Parsers;
using Workbench.Core.Repeaters;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Expression for binding a visualizer with data from the solution.
    /// </summary>
    [Serializable]
    public class VisualizerBindingExpressionModel : AbstractModel
    {
        private string text;

        /// <summary>
        /// Visualizer binding expression AST tree.
        /// </summary>
        [NonSerialized]
        private VisualizerBindingExpressionNode node;

        public VisualizerBindingExpressionModel(string rawExpression)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(rawExpression));
            Text = rawExpression;
        }

        public VisualizerBindingExpressionModel()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the raw visualizer binding expression.
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                ParseUnit(this.text);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the visualizer binding expression root node.
        /// </summary>
        public VisualizerBindingExpressionNode Node
        {
            get { return node; }
            private set { node = value; }
        }

        /// <summary>
        /// Execute the visualizer binding expression.
        /// </summary>
        /// <param name="theContext">Context for updating the visualizer.</param>
        public void ExecuteWith(VisualizerUpdateContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            if (Node == null) return;
            var repeater = new VisualizerRepeater(theContext.Snapshot);
            repeater.Process(repeater.CreateContextFrom(theContext));
        }

        /// <summary>
        /// Parse the raw visualizer binding expression.
        /// </summary>
        /// <param name="rawExpression">Raw binding expression.</param>
        private void ParseUnit(string rawExpression)
        {
            if (!string.IsNullOrWhiteSpace(rawExpression))
            {
                var parser = new VisualizerBindingExpressionParser();
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
