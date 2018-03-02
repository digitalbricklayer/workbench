using System;

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
        public AllDifferentConstraintModel(ModelName theName, AllDifferentConstraintExpressionModel theExpressionModel)
            : base(theName)
        {
            Expression = theExpressionModel;
        }

        public AllDifferentConstraintModel(AllDifferentConstraintExpressionModel theExpressionModel)
        {
            Expression = theExpressionModel;
        }

        public AllDifferentConstraintModel()
        {
            Expression = new AllDifferentConstraintExpressionModel();
        }

        /// <summary>
        /// Gets or sets the expression model.
        /// </summary>
        public AllDifferentConstraintExpressionModel Expression { get; set; }

        /// <summary>
        /// Validate the all different constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel)
        {
            return Validate(theModel, new ModelValidationContext());
        }

        /// <summary>
        /// Validate the all different constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel, ModelValidationContext theContext)
        {
            return !string.IsNullOrWhiteSpace(Expression.Text);
        }
    }
}
