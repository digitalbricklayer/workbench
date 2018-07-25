using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class VariableModelItemViewModel : ModelItemViewModel
    {
        private VariableDomainExpressionEditorViewModel _domainExpression;
        private string _domainExpressionText;

        protected VariableModelItemViewModel(VariableModel theVariableModel)
            : base(theVariableModel)
        {
            Variable = theVariableModel;
            DisplayName = Variable.Name;
            DomainExpressionText = theVariableModel.DomainExpression.Text;
            _domainExpression = new VariableDomainExpressionEditorViewModel(theVariableModel.DomainExpression);
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

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public VariableDomainExpressionEditorViewModel DomainExpression
        {
            get
            {
                return _domainExpression;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _domainExpression = value;
                if (Variable != null)
                    Variable.DomainExpression = _domainExpression.Model;
                NotifyOfPropertyChange();
            }
        }

        public VariableModel Variable { get; set; }
    }
}