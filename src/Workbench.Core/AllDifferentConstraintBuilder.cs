using Workbench.Core.Models;

namespace Workbench.Core
{
    public class AllDifferentConstraintBuilder
    {
        private AllDifferentConstraintExpressionModel expression = new AllDifferentConstraintExpressionModel();
        private ModelName name = new ModelName("New Constraint");
        private ModelModel _model;

        public AllDifferentConstraintBuilder WithName(string theName)
        {
            this.name = new ModelName(theName);
            return this;
        }

        public AllDifferentConstraintBuilder WithExpression(string theExpression)
        {
            this.expression = new AllDifferentConstraintExpressionModel(theExpression);
            return this;
        }

        public AllDifferentConstraintBuilder Inside(ModelModel theModel)
        {
            _model = theModel;
            return this;
        }

        public AllDifferentConstraintModel Build()
        {
            return new AllDifferentConstraintModel(_model, this.name, this.expression);
        }
    }
}
