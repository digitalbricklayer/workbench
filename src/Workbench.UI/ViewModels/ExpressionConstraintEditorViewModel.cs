using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class ExpressionConstraintEditorViewModel : ConstraintEditorViewModel
    {
        private ExpressionConstraintGraphicModel model;

        public ExpressionConstraintEditorViewModel(ExpressionConstraintGraphicModel theExpressionConstraintGraphic, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theExpressionConstraintGraphic, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theExpressionConstraintGraphic != null);

            Model = theExpressionConstraintGraphic;
            Expression = new ConstraintExpressionEditorViewModel(Model.Expression);
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionEditorViewModel Expression { get; private set; }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public new ExpressionConstraintGraphicModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }
    }
}