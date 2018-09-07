using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class VariableModelItemViewModel : ModelItemViewModel
    {
        private string _domainExpressionText;

        protected VariableModelItemViewModel(VariableModel theVariableModel)
            : base(theVariableModel)
        {
            Variable = theVariableModel;
            DisplayName = Variable.Name;
            DomainExpressionText = theVariableModel.DomainExpression.Text;
        }

        public string DomainExpressionText
        {
            get { return _domainExpressionText; }
            set
            {
                _domainExpressionText = value;
                NotifyOfPropertyChange();
            }
        }

        public VariableModel Variable { get;}
    }
}