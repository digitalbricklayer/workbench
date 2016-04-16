using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Base class for all constraint types.
    /// </summary>
    public abstract class ConstraintModel : GraphicModel
    {
        /// <summary>
        /// Initialize the constraint with a constraint name and location.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Location of the graphic.</param>
        protected ConstraintModel(string constraintName, Point location)
            : base(constraintName, location)
        {
        }

        /// <summary>
        /// Initialize the constraint with a constraint name.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        protected ConstraintModel(string constraintName)
            : base(constraintName)
        {
        }

        /// <summary>
        /// Initialize the constraint with default values.
        /// </summary>
        protected ConstraintModel()
            : base("new constraint")
        {
        }
    }
}
