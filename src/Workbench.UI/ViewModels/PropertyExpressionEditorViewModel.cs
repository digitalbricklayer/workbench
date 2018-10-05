using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the property expression editor dialog box.
    /// </summary>
    public class PropertyExpressionEditorViewModel : Screen
    {
        private string _expression;

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        public string Expression
        {
            get => this._expression;
            set
            {
                _expression = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void AcceptButton()
        {
            TryClose(true);
        }
    }
}
