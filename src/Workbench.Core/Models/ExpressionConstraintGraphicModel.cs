using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An expression constraint graphic representing an expression constraint.
    /// </summary>
    [Serializable]
    public class ExpressionConstraintGraphicModel : ConstraintGraphicModel
    {
        private ExpressionConstraintModel constraint;

        /// <summary>
        /// Initialize a constraint with a name and a constraint expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Location of the graphic.</param>
        /// <param name="theConstraint">Expression constraint model.</param>
        public ExpressionConstraintGraphicModel(string constraintName, Point location, ExpressionConstraintModel theConstraint)
            : base(constraintName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentNullException>(theConstraint != null);
            this.constraint = theConstraint;
        }

        /// <summary>
        /// Initialize a constraint with a name and constraint expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ExpressionConstraintGraphicModel(string constraintName, string rawExpression)
            : base(constraintName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            this.constraint = new ExpressionConstraintModel(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with a raw constraint expression.
        /// </summary>
        public ExpressionConstraintGraphicModel()
            : base("New constraint")
        {
            this.constraint = new ExpressionConstraintModel();
        }

        /// <summary>
        /// Gets the expression constraint model.
        /// </summary>
        public ExpressionConstraintModel Constraint { get { return this.constraint; } }

        /// <summary>
        /// Gets the constraint expression.
        /// </summary>
        public ConstraintExpressionModel Expression { get { return this.constraint.Expression; } }

        /// <summary>
        /// Parse the raw expression text.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="expressionText">Raw constraint expression.</param>
        public static ExpressionConstraintGraphicModel ParseExpression(string constraintName, string expressionText)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(expressionText));

            return new ExpressionConstraintGraphicModel(constraintName, expressionText);
        }

        public override bool Validate(ModelModel theModel)
        {
            return this.constraint.Validate(theModel);
        }

        public override bool Validate(ModelModel theModel, ModelValidationContext theContext)
        {
            return this.constraint.Validate(theModel, theContext);
        }
    }
}
