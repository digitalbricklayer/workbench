using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An "all different" constraint graphic.
    /// </summary>
    [Serializable]
    public sealed class AllDifferentConstraintGraphicModel : ConstraintGraphicModel
    {
        private AllDifferentConstraintModel constraint;

        public AllDifferentConstraintGraphicModel(string theConstraintName, Point theLocation, AllDifferentConstraintModel theConstraint)
            : base(theConstraintName, theLocation)
        {
            Contract.Requires<ArgumentNullException>(theConstraint != null);
            this.constraint = theConstraint;
        }

        /// <summary>
        /// Initialize the all different constraint model with a name and location.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Constraint screen location.</param>
        public AllDifferentConstraintGraphicModel(string constraintName, Point location)
            : base(constraintName, location)
        {
            this.constraint = new AllDifferentConstraintModel();
        }

        /// <summary>
        /// Gets the all different constraint model.
        /// </summary>
        public AllDifferentConstraintModel Constraint { get { return this.constraint; } }

        /// <summary>
        /// Gets the all different constraint expression.
        /// </summary>
        public AllDifferentConstraintExpressionModel Expression
        {
            get
            {
                Contract.Assume(this.constraint != null);

                return this.constraint.Expression;
            }
        }

        /// <summary>
        /// Validate the all different constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel)
        {
            return this.constraint.Validate(theModel);
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
            return this.constraint.Validate(theModel, theContext);
        }
    }
}
