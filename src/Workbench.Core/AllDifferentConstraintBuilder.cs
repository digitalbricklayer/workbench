using Workbench.Core.Models;

namespace Workbench.Core
{
    public class AllDifferentConstraintBuilder
    {
        private AllDifferentConstraintExpressionModel expression = new AllDifferentConstraintExpressionModel();
        private ModelName name = new ModelName("New Constraint");
        private BundleModel _bundle;

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

        public AllDifferentConstraintBuilder Inside(BundleModel theBundle)
        {
            _bundle = theBundle;
            return this;
        }

        public AllDifferentConstraintModel Build()
        {
            return new AllDifferentConstraintModel(_bundle, this.name, this.expression);
        }
    }
}
