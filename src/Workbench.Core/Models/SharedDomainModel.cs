using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public class SharedDomainModel : Model
    {
        private SharedDomainExpressionModel _expression;

        /// <summary>
        /// Initialize a shared domain with a name and domain expression.
        /// </summary>
        public SharedDomainModel(ModelName theName, SharedDomainExpressionModel theExpression)
            : base(theName)
        {
            Contract.Requires<ArgumentNullException>(theName != null);
            Contract.Requires<ArgumentNullException>(theExpression != null);
            _expression = theExpression;
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public SharedDomainExpressionModel Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Validate the shared domain placing errors into the validation context.
        /// </summary>
        /// <returns>
        /// Return true if the domain is valid, return false if 
        /// the domain is not valid.
        /// </returns>
        public bool Validate(ModelModel theModel, ModelValidationContext validateContext)
        {
            if (Expression.Node == null) return false;

            return ValidateTableReferences(theModel, validateContext);
        }

        private bool ValidateTableReferences(ModelModel theModel, ModelValidationContext theContext)
        {
            var variableCaptureVisitor = new TableCellReferenceCaptureVisitor();
            Expression.Node.AcceptVisitor(variableCaptureVisitor);

            // Make sure all of the table references are valid
            var theWorkspace = theModel.Workspace;
            foreach (var aTableReference in variableCaptureVisitor.GetReferences())
            {
                var tableName = aTableReference.Name;
                var theVisualizer = theWorkspace.GetVisualizerBy(tableName);
                /*
                 * The visualizer isn't guaranteed to be a table tab, referencing
                 * a visualizer that isn't a table doesn't make much sense in a
                 * domain expression.
                 */
                if (!(theVisualizer is TableTabModel))
                {
                    theContext.AddError($"Missing table {tableName}");
                    return false;
                }
            }

            return true;
        }
    }
}
