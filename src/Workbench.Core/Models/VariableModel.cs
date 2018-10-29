using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A variable can hold a value constrained by a constraint.
    /// </summary>
    [Serializable]
    public abstract class VariableModel : Model
    {
        private WorkspaceModel workspace;
        private ModelModel model;
        private VariableDomainExpressionModel domainExpression;

        /// <summary>
        /// Initializes a variable with a workspace, variable name and domain expression.
        /// </summary>
        protected VariableModel(WorkspaceModel theModel, ModelName variableName, VariableDomainExpressionModel theDomainExpression)
            : base(variableName)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(variableName != null);
            Contract.Requires<ArgumentNullException>(theDomainExpression != null);

            Workspace = theModel;
            Model = theModel.Model;
            DomainExpression = theDomainExpression;
        }

        /// <summary>
        /// Initializes a variable with a variable name.
        /// </summary>
        protected VariableModel(ModelModel theModel, ModelName variableName)
            : base(variableName)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(variableName != null);

            Model = theModel;
            DomainExpression = new VariableDomainExpressionModel();
        }

        /// <summary>
        /// Gets or sets the variable domain expression.
        /// </summary>
        public VariableDomainExpressionModel DomainExpression
        {
            get { return domainExpression; }
            set
            {
                domainExpression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the workspace the variable is assigned.
        /// </summary>
        public WorkspaceModel Workspace
        {
            get { return this.workspace; }
            internal set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.workspace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Get the model that the variable is assigned.
        /// </summary>
        public ModelModel Model
        {
            get { return this.model; }
            internal set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.model = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Returns a string that represents the variable.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name.Text;
        }

        /// <summary>
        /// Get the size of the variable.
        /// </summary>
        /// <returns>Size of the variable.</returns>
        public abstract long GetSize();

        /// <summary>
        /// Get the variable domain band.
        /// </summary>
        /// <returns>Domain value.</returns>
        public virtual DomainValue GetVariableBand()
        {
            return VariableBandEvaluator.GetVariableBand(this);
        }

        public override void AssignIdentity()
        {
            base.AssignIdentity();
            DomainExpression.AssignIdentity();
        }

        /// <summary>
        /// Validate the variable.
        /// </summary>
        /// <returns>
        /// Return true if the variable is valid, return false if 
        /// the variable is not valid.
        /// </returns>
        public bool Validate(ModelModel theModel)
        {
            return Validate(theModel, new ModelValidationContext());
        }

        /// <summary>
        /// Validate the variable placing errors into the validation context.
        /// </summary>
        /// <returns>
        /// Return true if the variable is valid, return false if 
        /// the variable is not valid.
        /// </returns>
        public bool Validate(ModelModel theModel, ModelValidationContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            Contract.Requires<ArgumentNullException>(theContext != null);

            if (DomainExpression.Node == null) return false;

            var tableReferenceValid = ValidateTableReferences(theModel, theContext);
            if (!tableReferenceValid) return false;
            return ValidateSharedDomainReferences(theModel, theContext);
        }

        private bool ValidateSharedDomainReferences(ModelModel theModel, ModelValidationContext validateContext)
        {
            if (DomainExpression == null)
            {
                validateContext.AddError("Missing domain");
                return false;
            }

            // Make sure the domain is a shared domain...
            if (DomainExpression.DomainReference == null)
                return true;

            var sharedDomain = theModel.GetSharedDomainByName(DomainExpression.DomainReference.DomainName.Name);
            if (sharedDomain == null)
            {
                validateContext.AddError($"Missing shared domain {DomainExpression.DomainReference.DomainName.Name}");
                return false;
            }

            return true;
        }

        private bool ValidateTableReferences(ModelModel theModel, ModelValidationContext theContext)
        {
            var variableCaptureVisitor = new TableCellReferenceCaptureVisitor();
            DomainExpression.Node.AcceptVisitor(variableCaptureVisitor);

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
