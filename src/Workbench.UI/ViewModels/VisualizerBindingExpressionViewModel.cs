using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class VisualizerBindingExpressionViewModel : Screen
    {
        public VisualizerBindingExpressionViewModel(VisualizerBindingExpressionModel theExpressionModel)
        {
            VisualizerExpression = theExpressionModel;
        }

        /// <summary>
        /// Gets the visualizer expression model.
        /// </summary>
        public VisualizerBindingExpressionModel VisualizerExpression { get; }

        /// <summary>
        /// Gets or sets the expression text.
        /// </summary>
        public string Text
        {
            get => VisualizerExpression.Text;
            set
            {
                VisualizerExpression.Text = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the visualizer expression identity.
        /// </summary>
        public int Id => VisualizerExpression.Id;
    }
}
