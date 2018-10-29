using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class ConstraintModel : Model
    {
        /// <summary>
        /// Initialize a constraint with a name.
        /// </summary>
        /// <param name="theName">Text.</param>
        protected ConstraintModel(ModelName theName)
            : base(theName)
        {
        }

        /// <summary>
        /// Initialize a constraint with a default name.
        /// </summary>
        protected ConstraintModel()
            : base(new ModelName())
        {
        }

        /// <summary>
        /// Validate the constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public virtual bool Validate(ModelModel theModel)
        {
            return Validate(theModel, new ModelValidationContext());
        }

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
