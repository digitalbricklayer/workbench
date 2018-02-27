using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class ExpressionConstraintVisualizerViewModel : ConstraintVisualizerViewModel
    {
        private ExpressionConstraintGraphicModel model;

        public ExpressionConstraintVisualizerViewModel(ExpressionConstraintModel theConstraint, ExpressionConstraintEditorViewModel theEditor, ExpressionConstraintViewerViewModel theViewer)
            : base(theConstraint, theEditor, theViewer)
        {
            ExpressionEditor = theEditor;
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionEditorViewModel Expression { get; private set; }

        /// <summary>
        /// Gets whether the expression is a valid expression.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        /// <summary>
        /// Gets or sets the constraint model.
        /// </summary>
        public new ExpressionConstraintGraphicModel Model
        {
            get { return this.model; }
            set
            {
                base.Model = value;
                this.model = value;
            }
        }

        public ExpressionConstraintEditorViewModel ExpressionEditor { get; }
    }
}
