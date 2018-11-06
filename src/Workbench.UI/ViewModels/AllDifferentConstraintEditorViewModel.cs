using Workbench.Validators;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintEditorViewModel : DialogViewModel
    {
        private string _constraintName;
        private string _constraintExpression;

        public AllDifferentConstraintEditorViewModel()
        {
            Validator = new AllDifferentConstraintEditorViewModelValidator();
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
    }
}