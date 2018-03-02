using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ConstraintViewerViewModel : ViewerViewModel
    {
        private ConstraintGraphicModel model;

        protected ConstraintViewerViewModel(ConstraintGraphicModel theConstraintGraphicModel)
            : base(theConstraintGraphicModel)
        {
        }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public ConstraintGraphicModel ConstraintGraphic
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