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
        private VariableViewModel variable;
        private AllDifferentConstraintModel model;
        private AllDifferentConstraintExpressionViewModel expression;

        public AllDifferentConstraintViewModel(AllDifferentConstraintModel theConstraintModel)
            : base(theConstraintModel)
        {
            Contract.Requires<ArgumentNullException>(theConstraintModel != null);
            base.Model = theConstraintModel;
            this.model = theConstraintModel;
            Expression = new AllDifferentConstraintExpressionViewModel(theConstraintModel.Expression);
        }

        /// <summary>
        /// Gets the variable the constraint is applied to.
        /// </summary>
        public VariableViewModel Variable
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
        public override ConstraintModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = (AllDifferentConstraintModel) value;
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
