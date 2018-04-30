using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public class VisualizerExpressionEditorViewModel : Screen
    {
        private string text;

        public VisualizerExpressionEditorViewModel(int id, string text)
        {
            Contract.Requires<ArgumentException>(id > 0);
            Contract.Requires<ArgumentException>(text != null);
            Id = id;
            Text = text;
        }

        /// <summary>
        /// Initialize a visualizer expression editor with a raw visualizer expression.
        /// </summary>
        /// <param name="rawVisualizerExpression">Raw visualizer expression.</param>
        public VisualizerExpressionEditorViewModel(string rawVisualizerExpression)
        {
            Text = rawVisualizerExpression;
        }

        /// <summary>
        /// Initialize a visualizer expression editor with default values.
        /// </summary>
        public VisualizerExpressionEditorViewModel()
        {
            Text = string.Empty;
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                NotifyOfPropertyChange();
            }
        }

        public int Id { get; private set; }
    }
}