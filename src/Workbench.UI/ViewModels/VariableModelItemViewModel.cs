using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Base class for the variable items in the model editor.
    /// </summary>
    public abstract class VariableModelItemViewModel : ModelItemViewModel
    {
        private string _domainExpressionText;

        protected VariableModelItemViewModel(VariableModel theVariableModel)
            : base(theVariableModel)
        {
            Variable = theVariableModel;
            DomainExpressionText = theVariableModel.DomainExpression.Text;
        }

        /// <summary>
        /// Gets or sets the domain expression text.
        /// </summary>
        public string DomainExpressionText
        {
            get { return _domainExpressionText; }
            set
            {
                _domainExpressionText = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the variable model.
        /// </summary>
        public VariableModel Variable { get;}

        protected override void OnInitialize()
        {
            DisplayName = Variable.Name;
            base.OnInitialize();
        }
    }
}