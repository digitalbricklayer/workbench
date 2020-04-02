using Workbench.Core.Models;

namespace Workbench.Core
{
    public class ExpressionConstraintBuilder
    {
        private ConstraintExpressionModel expression = new ConstraintExpressionModel();
        private ModelName name = new ModelName("New Constraint");
        private BundleModel _model;

        public ExpressionConstraintBuilder WithName(string theName)
        {
            this.name = new ModelName(theName);
            return this;
        }

        public ExpressionConstraintBuilder WithExpression(string theExpression)
        {
            this.expression = new ConstraintExpressionModel(theExpression);
            return this;
        }

        public ExpressionConstraintBuilder Inside(BundleModel theBundle)
        {
            _model = theBundle;
            return this;
        }

        public ExpressionConstraintModel Build()
        {
            return new ExpressionConstraintModel(_model, this.name, this.expression);
        }
    }
}
