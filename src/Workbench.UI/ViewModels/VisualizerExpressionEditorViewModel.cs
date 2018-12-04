using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the visualizer expression editor dialog box.
    /// </summary>
    public sealed class VisualizerExpressionEditorViewModel : DialogViewModel
    {
        private string _expression;

        /// <summary>
        /// Initialize a new visualizer expression editor view model with default values.
        /// </summary>
        public VisualizerExpressionEditorViewModel()
        {
            Validator = new VisualizerExpressionValidator();
            Expression = string.Empty;
        }

        /// <summary>
        /// Gets or sets the tab title.
        /// </summary>
        public string Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
