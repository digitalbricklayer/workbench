using System;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Base class for all constraint types.
    /// </summary>
    [Serializable]
    public abstract class ConstraintGraphicModel : GraphicModel
    {
        /// <summary>
        /// Initialize the constraint graphic with a constraint and location.
        /// </summary>
        /// <param name="location">Location of the graphic.</param>
        protected ConstraintGraphicModel(ConstraintModel theConstraint, Point location)
            : base(theConstraint, location)
        {
            Constraint = theConstraint;
        }

        /// <summary>
        /// Initialize the constraint graphic with a constraint.
        /// </summary>
        protected ConstraintGraphicModel(ConstraintModel theConstraint)
            : base(theConstraint)
        {
            Constraint = theConstraint;
        }

        /// <summary>
        /// Gets the constraint model.
        /// </summary>
        public ConstraintModel Constraint { get; private set; }

        /// <summary>
        /// Validate the constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public abstract bool Validate(ModelModel theModel);

        /// <summary>
        /// Validate the constraint placing errors into the validation context.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public abstract bool Validate(ModelModel theModel, ModelValidationContext theContext);
    }
}
