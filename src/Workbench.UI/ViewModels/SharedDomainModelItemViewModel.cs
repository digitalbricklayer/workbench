using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Validators;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class SharedDomainModelItemViewModel : ModelItemViewModel
    {
        private readonly IWindowManager _windowManager;
        private string _expressionText;

        public SharedDomainModelItemViewModel(SharedDomainModel theDomain, IWindowManager theWindowManager)
            : base(theDomain)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            Validator = new SharedDomainModelItemViewModelValidator();
            Domain = theDomain;
            DisplayName = theDomain.Name;
            ExpressionText = theDomain.Expression.Text;
            _windowManager = theWindowManager;
        }

        public SharedDomainModel Domain { get; private set; }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public string ExpressionText
        {
            get { return _expressionText; }
            set
            {
                Set(ref _expressionText, value);
            }
        }

        public override void Edit()
        {
            var domainEditorViewModel = new SharedDomainEditorViewModel();
            domainEditorViewModel.DomainName = Domain.Name;
            domainEditorViewModel.DomainExpression = Domain.Expression.Text;
            var result = _windowManager.ShowDialog(domainEditorViewModel);
            if (!result.GetValueOrDefault()) return;
            DisplayName = Domain.Name.Text = domainEditorViewModel.DomainName;
            ExpressionText = Domain.Expression.Text = domainEditorViewModel.DomainExpression;
        }
    }
}
