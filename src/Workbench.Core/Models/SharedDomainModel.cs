using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public class SharedDomainModel : DomainModel
    {
        private SharedDomainExpressionModel _expression;
        private ModelModel _parent;

        /// <summary>
        /// Initialize a shared domain with a name and domain expression.
        /// </summary>
        public SharedDomainModel(ModelModel theModel, ModelName theName, SharedDomainExpressionModel theExpression)
            : base(theName)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theName != null);
            Contract.Requires<ArgumentNullException>(theExpression != null);
            Parent = theModel;
            _expression = theExpression;
        }

        /// <summary>
        /// Gets the parent model.
        /// </summary>
        public ModelModel Parent
        {
            get => _parent;
            private set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _parent = value;
            }
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
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(validateContext != null);

            // The Node will not be null when the parser has created the AST root node
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
