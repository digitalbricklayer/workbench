using System;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Visualizer binding expression item.
    /// </summary>
    public class VisualizerExpressionItemViewModel : Screen
    {
        private string _text;
        private int _id;

        /// <summary>
        /// Initialize a visualizer expression editor with an identity and raw visualizer expression text.
        /// </summary>
        /// <param name="id">Visualizer binding identity.</param>
        /// <param name="text">Raw visualizer expression text.</param>
        public VisualizerExpressionItemViewModel(int id, string text)
        {
            if (id <= 0)
                throw new ArgumentException("Identifier must be nonzero", nameof(id));

            Id = id;
            Text = text;
        }

        /// <summary>
        /// Initialize a visualizer expression editor with a raw visualizer expression.
        /// </summary>
        /// <param name="rawVisualizerExpression">Raw visualizer expression.</param>
        public VisualizerExpressionItemViewModel(string rawVisualizerExpression)
        {
            Text = rawVisualizerExpression;
        }

        /// <summary>
        /// Initialize a visualizer expression editor with default values.
        /// </summary>
        public VisualizerExpressionItemViewModel()
        {
            Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the visualizer binding text.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the visualizer binding identity.
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                NotifyOfPropertyChange();
            }
        }
    }
}