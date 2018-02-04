using System;
using System.Diagnostics.Contracts;
using System.Linq;

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

        public ExpressionConstraintModel(ModelName theName, ConstraintExpressionModel theExpression)
            : base(theName)
        {
            this.expression = theExpression;
        }

        public ExpressionConstraintModel(ModelName theName)
            : base(theName)
        {
            this.expression = new ConstraintExpressionModel();
        }

        public ExpressionConstraintModel(ConstraintExpressionModel theExpression)
            : base(new ModelName())
        {
            this.expression = theExpression;
        }

        public ExpressionConstraintModel()
            : base(new ModelName())
        {
            this.expression = new ConstraintExpressionModel();
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
        /// Validate the constraint expression.
        /// </summary>
        /// <param name="theModel">Model to validate.</param>
        /// <param name="theContext">Validation context to capture the errors.</param>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel, ModelValidationContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theContext != null);

            if (Expression.Node == null) return false;

            var variableCaptureVisitor = new ConstraintVariableReferenceCaptureVisitor();
            Expression.Node.AcceptVisitor(variableCaptureVisitor);
            var variableReferences = variableCaptureVisitor.GetReferences();

            foreach (var singletonVariableReference in variableReferences.SingletonVariableReferences)
            {
                if (theModel.Variables.FirstOrDefault(_ => _.Name == singletonVariableReference.VariableName) == null)
                {
                    theContext.AddError($"Missing singleton variable {singletonVariableReference.VariableName}");
                    return false;
                }
            }

            foreach (var aggregateVariableReference in variableReferences.AggregateVariableReferences)
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
