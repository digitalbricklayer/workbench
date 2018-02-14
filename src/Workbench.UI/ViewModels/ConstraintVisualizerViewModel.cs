using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ConstraintVisualizerViewModel : VisualizerViewModel
    {
        private ConstraintGraphicModel model;

        protected ConstraintVisualizerViewModel(ConstraintModel theConstraint, ConstraintEditorViewModel theEditor, ConstraintViewerViewModel theViewer)
            : base(theConstraint, theEditor, theViewer)
        {
        }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public new virtual ConstraintGraphicModel Model
        {
            get { return this.model; }
            set
            {
//                base.Model = value;
                this.model = value;
            }
        }
    }
}
