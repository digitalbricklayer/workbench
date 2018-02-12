using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for an all different constraint.
    /// </summary>
    public class AllDifferentConstraintViewModel : ConstraintViewModel
    {
        private VariableGraphicViewModel variable;
        private AllDifferentConstraintGraphicModel model;
        private AllDifferentConstraintExpressionViewModel expression;

        public AllDifferentConstraintViewModel(AllDifferentConstraintGraphicModel theGraphicModel)
            : base(theGraphicModel)
        {
            Contract.Requires<ArgumentNullException>(theGraphicModel != null);
            base.Model = theGraphicModel;
            this.model = theGraphicModel;
            Expression = new AllDifferentConstraintExpressionViewModel(theGraphicModel.Expression);
        }

        /// <summary>
        /// Gets the variable the constraint is applied to.
        /// </summary>
        public VariableGraphicViewModel Variable
        {
            get { return this.variable; }
            set
            {
                this.variable = value;
                NotifyOfPropertyChange();
            }
        }

        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        /// <summary>
        /// Gets or sets the all different constraint model.
        /// </summary>
        public override ConstraintGraphicModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = (AllDifferentConstraintGraphicModel) value;
            }
        }

        public AllDifferentConstraintExpressionViewModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
