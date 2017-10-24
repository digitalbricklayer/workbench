using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for an expression based constraint.
    /// </summary>
    public sealed class ExpressionConstraintViewModel : ConstraintViewModel
    {
        private ExpressionConstraintGraphicModel model;

        public ExpressionConstraintViewModel(ExpressionConstraintGraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            Contract.Requires<ArgumentNullException>(theGraphicModel != null);
            Model = theGraphicModel;
            Expression = new ConstraintExpressionViewModel(Model.Expression);
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionViewModel Expression { get; private set; }

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
