using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for a domain.
    /// </summary>
    public sealed class DomainEditorViewModel : EditorViewModel
    {
        private DomainGraphicModel model;
        private DomainExpressionEditorViewModel expression;

        public DomainEditorViewModel(DomainGraphicModel theDomainGraphic, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theDomainGraphic, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theDomainGraphic != null);
            DomainGraphic = theDomainGraphic;
            Expression = new DomainExpressionEditorViewModel(theDomainGraphic.Expression);
        }

        public DomainGraphicModel DomainGraphic { get; set; }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionEditorViewModel Expression
        {
            get { return this.expression; }
            set
            {
                Set(ref this.expression, value);
            }
        }

        /// <summary>
        /// Gets whether the domain expression is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(Expression);
        }
    }
}
