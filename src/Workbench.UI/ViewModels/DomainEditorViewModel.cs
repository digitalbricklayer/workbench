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

        public DomainEditorViewModel(DomainGraphicModel theDomainGraphic, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theDomainGraphic, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theDomainGraphic != null);
            DomainGraphic = theDomainGraphic;
            this.Expression = new DomainExpressionViewModel(theDomainGraphic.Expression);
        }

        public DomainGraphicModel DomainGraphic { get; set; }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionViewModel Expression { get; set; }

        /// <summary>
        /// Get whether the domain expression is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Expression.Text);
            }
        }
    }
}
