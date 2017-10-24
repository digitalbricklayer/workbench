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
        /// Initialize the constraint with a constraint name and location.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Location of the graphic.</param>
        protected ConstraintGraphicModel(string constraintName, Point location)
            : base(constraintName, location)
        {
        }

        /// <summary>
        /// Initialize the constraint with a constraint name.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        protected ConstraintGraphicModel(string constraintName)
            : base(constraintName)
        {
        }

        /// <summary>
        /// Initialize the constraint with default values.
        /// </summary>
        protected ConstraintGraphicModel()
            : base("new constraint")
        {
        }

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
