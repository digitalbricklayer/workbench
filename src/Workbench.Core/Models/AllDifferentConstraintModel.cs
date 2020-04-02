using System;
using System.Linq;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An "all different" constraint for an aggregate variable.
    /// </summary>
    /// <remarks>
    /// Makes very little sense to have an all different constraint tied to 
    /// anything other than an aggregate at the moment. Maybe later we could 
    /// implement the constraint for 2 or more singletons.
    /// </remarks>
    [Serializable]
    public class AllDifferentConstraintModel : ConstraintModel
    {
        private AllDifferentConstraintExpressionModel _expression;

        public AllDifferentConstraintModel(BundleModel bundle, ModelName theName, AllDifferentConstraintExpressionModel theExpressionModel)
            : base(theName)
        {
            Parent = bundle;
            Expression = theExpressionModel;
        }

        public AllDifferentConstraintModel(BundleModel bundle, AllDifferentConstraintExpressionModel theExpressionModel)
        {
            Parent = bundle;
            Expression = theExpressionModel;
        }

        public AllDifferentConstraintModel(BundleModel bundle)
        {
            Parent = bundle;
            Expression = new AllDifferentConstraintExpressionModel();
        }

        /// <summary>
        /// Gets or sets the expression model.
        /// </summary>
        public AllDifferentConstraintExpressionModel Expression
        {
            get => _expression;
            set
            {
                _expression = value;
            }
        }

        /// <summary>
        /// Validate the all different constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(BundleModel bundle, ModelValidationContext theContext)
        {
            if (string.IsNullOrWhiteSpace(Expression.Text))
            {
                theContext.AddError("Missing aggregate variable");
                return false;
            }

            if (bundle.Aggregates.FirstOrDefault(_ => _.Name.IsEqualTo(Expression.Text)) == null)
            {
                theContext.AddError($"Missing aggregate variable {Expression.Text}");
                return false;
            }

            return true;
        }
    }
}
