using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Base class for all constraint view models.
    /// </summary>
    public abstract class ConstraintViewModel : GraphicViewModel
    {
        private ConstraintModel model;

        protected ConstraintViewModel(ConstraintModel theGraphicModel)
            : base(theGraphicModel)
        {
        }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public virtual new ConstraintModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }
    }
}
