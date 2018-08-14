using Workbench.Core.Models;

namespace Workbench.Core
{
    public class ExpressionConstraintBuilder
    {
        private ConstraintExpressionModel expression = new ConstraintExpressionModel();
        private ModelName name = new ModelName("New Constraint");

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

        public ExpressionConstraintModel Build()
        {
            return new ExpressionConstraintModel(this.name, this.expression);
        }
    }
}
