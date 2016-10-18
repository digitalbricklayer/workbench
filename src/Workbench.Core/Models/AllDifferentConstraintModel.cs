using System;
using System.Diagnostics.Contracts;
using System.Windows;

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
    public sealed class AllDifferentConstraintModel : ConstraintModel
    {
        public AllDifferentConstraintModel(string constraintName, Point location, AllDifferentConstraintExpressionModel theExpressionModel)
            : base(constraintName, location)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
            Expression = theExpressionModel;
        }

        /// <summary>
        /// Initialize the all different constraint model with a name and location.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Constraint screen location.</param>
        public AllDifferentConstraintModel(string constraintName, Point location)
            : base(constraintName, location)
        {
            Expression = new AllDifferentConstraintExpressionModel();
        }

        public AllDifferentConstraintModel(AllDifferentConstraintExpressionModel theExpressionModel)
        {
            Contract.Requires<ArgumentNullException>(theExpressionModel != null);
            Expression = theExpressionModel;
        }

        public AllDifferentConstraintModel()
        {
            Expression = new AllDifferentConstraintExpressionModel();
        }

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
