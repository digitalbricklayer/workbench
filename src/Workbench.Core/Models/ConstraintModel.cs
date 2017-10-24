using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class ConstraintModel : AbstractModel
    {
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
