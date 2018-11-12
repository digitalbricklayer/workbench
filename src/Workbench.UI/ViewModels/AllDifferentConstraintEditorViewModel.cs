using System.Collections.ObjectModel;
using Workbench.Core.Models;
using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// All different constraint editor view model.
    /// </summary>
    public class AllDifferentConstraintEditorViewModel : DialogViewModel
    {
        private string _constraintName;
        private string _selectedVariable;
        private ObservableCollection<string> _variables;

        /// <summary>
        /// Initialize an all different constraint editor view model with the model.
        /// </summary>
        public AllDifferentConstraintEditorViewModel(ModelModel theModel)
        {
            Validator = new AllDifferentConstraintEditorViewModelValidator();
            ConstraintName = string.Empty;
            SelectedVariable = string.Empty;
            Variables = new ObservableCollection<string>();
            Variables.Add(string.Empty);
            foreach (var anAggregateVariable in theModel.Aggregates)
            {
                Variables.Add(anAggregateVariable.Name);
            }
        }

        /// <summary>
        /// Gets or sets the constraint name.
        /// </summary>
        public string ConstraintName
        {
            get => _constraintName;
            set
            {
                _constraintName = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the selected variable name.
        /// </summary>
        public string SelectedVariable
        {
            get => _selectedVariable;
            set
            {
                _selectedVariable = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets all aggregate variables that the constraint can be bound.
        /// </summary>
        public ObservableCollection<string> Variables
        {
            get => _variables;
            set
            {
                _variables = value; 
                NotifyOfPropertyChange();
            }
        }
    }
}