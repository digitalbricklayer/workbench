using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class DomainModelItemViewModel : ModelItemViewModel
    {
        private readonly IWindowManager _windowManager;

        private string _expressionText;

        public DomainModelItemViewModel(SharedDomainModel theDomain, IWindowManager theWindowManager)
            : base(theDomain)
        {
            Contract.Requires<ArgumentNullException>(theDomain != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            Domain = theDomain;
            DisplayName = theDomain.Name;
            ExpressionText = theDomain.Expression.Text;
            _windowManager = theWindowManager;
        }

        public SharedDomainModel Domain { get; set; }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public string ExpressionText
        {
            get { return this._expressionText; }
            set
            {
                Set(ref this._expressionText, value);
            }
        }

        public override void Edit()
        {
            var domainEditorViewModel = new DomainEditorViewModel();
            domainEditorViewModel.DomainName = Domain.Name;
            domainEditorViewModel.DomainExpression = Domain.Expression.Text;
            var result = _windowManager.ShowDialog(domainEditorViewModel);
            if (!result.HasValue) return;
            DisplayName = Domain.Name.Text = domainEditorViewModel.DomainName;
            ExpressionText = Domain.Expression.Text = domainEditorViewModel.DomainExpression;
        }
    }
}
