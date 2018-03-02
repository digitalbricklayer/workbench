using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintViewerViewModel : ConstraintViewerViewModel
    {
        private VariableEditorViewModel variable;
        private AllDifferentConstraintGraphicModel model;
        private AllDifferentConstraintExpressionViewerViewModel expression;

        public AllDifferentConstraintViewerViewModel(AllDifferentConstraintGraphicModel theAllDifferentGraphicModel)
            : base(theAllDifferentGraphicModel)
        {
            AllDifferentConstraintGraphic = theAllDifferentGraphicModel;
            Expression = new AllDifferentConstraintExpressionViewerViewModel(theAllDifferentGraphicModel.Expression);
        }

        public AllDifferentConstraintGraphicModel AllDifferentConstraintGraphic { get; }

        /// <summary>
        /// Gets the variable the constraint is applied to.
        /// </summary>
        public VariableEditorViewModel Variable
        {
            get { return this.variable; }
            set
            {
                this.variable = value;
                NotifyOfPropertyChange();
            }
        }

        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        public AllDifferentConstraintExpressionViewerViewModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                NotifyOfPropertyChange();
            }
        }
    }
}