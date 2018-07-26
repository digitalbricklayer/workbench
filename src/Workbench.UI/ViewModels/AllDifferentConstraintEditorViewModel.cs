using Caliburn.Micro;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintEditorViewModel : Screen
    {
        private string _constraintName;
        private string _constraintExpression;

        public AllDifferentConstraintEditorViewModel()
        {
            ConstraintName = string.Empty;
            ConstraintExpression = string.Empty;
        }

        public string ConstraintName
        {
            get => _constraintName;
            set
            {
                _constraintName = value;
                NotifyOfPropertyChange();
            }
        }

        public string ConstraintExpression
        {
            get => _constraintExpression;
            set
            {
                _constraintExpression = value;
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