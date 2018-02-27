using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class ExpressionConstraintViewerViewModel : ConstraintViewerViewModel
    {
        private ExpressionConstraintGraphicModel model;

        public ExpressionConstraintViewerViewModel(ExpressionConstraintGraphicModel theExpressionConstraintGraphicModel)
            : base(theExpressionConstraintGraphicModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionConstraintGraphicModel != null);

            Model = theExpressionConstraintGraphicModel;
            Expression = new ConstraintExpressionViewerViewModel(Model.Expression);
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionViewerViewModel Expression { get; private set; }

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