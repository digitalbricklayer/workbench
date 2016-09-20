using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A constraint restricts the values that can be bound to a variable 
    /// through an expression entered by the user of the program.
    /// </summary>
    [Serializable]
    public class ExpressionConstraintModel : ConstraintModel
    {
        private ConstraintExpressionModel expression;

        /// <summary>
        /// Initialize a constraint with a name and a constraint expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="location">Location of the graphic.</param>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public ExpressionConstraintModel(string constraintName, Point location, ConstraintExpressionModel theExpression)
            : base(constraintName, location)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.Expression = theExpression;
        }

        /// <summary>
        /// Initialize a constraint with a name and constraint expression.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ExpressionConstraintModel(string constraintName, string rawExpression)
            : base(constraintName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with a constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        public ExpressionConstraintModel(string rawExpression)
            : this()
        {
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            this.Expression = new ConstraintExpressionModel(rawExpression);
        }

        /// <summary>
        /// Initialize a constraint with an expression.
        /// </summary>
        /// <param name="theExpression">Binary expression expressing the constraint.</param>
        public ExpressionConstraintModel(ConstraintExpressionModel theExpression)
            : this()
        {
            Contract.Requires<ArgumentNullException>(theExpression != null);
            this.Expression = theExpression;
        }

        /// <summary>
        /// Initialize a constraint with a raw constraint expression.
        /// </summary>
        public ExpressionConstraintModel()
            : base("New constraint")
        {
            this.Expression = new ConstraintExpressionModel();
        }

        /// <summary>
        /// Gets or sets the constraint expression.
        /// </summary>
        public ConstraintExpressionModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Parse the raw expression text.
        /// </summary>
        /// <param name="constraintName">Constraint name.</param>
        /// <param name="expressionText">Raw constraint expression.</param>
        public static ExpressionConstraintModel ParseExpression(string constraintName, string expressionText)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(constraintName));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(expressionText));

            return new ExpressionConstraintModel(constraintName, expressionText);
        }

        /// <summary>
        /// Validate the constraint.
        /// </summary>
        /// <param name="theModel">Model to validate.</param>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel)
        {
            return Validate(theModel, new ModelValidationContext());
        }

        /// <summary>
        /// Validate the constraint.
        /// </summary>
        /// <param name="theModel">Model to validate.</param>
        /// <param name="theContext">Validation context to capture the errors.</param>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel, ModelValidationContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);

            if (Expression.Node == null) return false;

            var validatorVisitor = new ConstraintExpressionValidatorVisitor();
            Expression.Node.Accept(validatorVisitor);

            foreach (var singletonVariableReference in validatorVisitor.SingletonVariableReferences)
            {
                if (theModel.Variables.FirstOrDefault(_ => _.Name == singletonVariableReference.VariableName) == null)
                {
                    theContext.AddError($"Missing singleton variable {singletonVariableReference.VariableName}");
                    return false;
                }
            }

            foreach (var aggregateVariableReference in validatorVisitor.AggregateVariableReferences)
            {
                if (theModel.Aggregates.FirstOrDefault(_ => _.Name == aggregateVariableReference.VariableName) == null)
                {
                    theContext.AddError($"Missing aggregate variable {aggregateVariableReference.VariableName}");
                    return false;
                }
            }

            return true;
        }
    }
}
