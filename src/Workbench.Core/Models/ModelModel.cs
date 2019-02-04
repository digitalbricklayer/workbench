using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A model for specifying the problem.
    /// <remarks>Just a very simple finite integer domain at the moment.</remarks>
    /// </summary>
    [Serializable]
    public sealed class ModelModel : Model
    {
        private WorkspaceModel workspace;
        private ObservableCollection<VariableModel> variables;
        private ObservableCollection<SingletonVariableModel> singletons;
        private ObservableCollection<AggregateVariableModel> aggregates;
        private ObservableCollection<SharedDomainModel> _sharedDomains;
        private ObservableCollection<ConstraintModel> constraints;
        private ObservableCollection<BundleModel> bundles;
        private ObservableCollection<BucketModel> _buckets;

        /// <summary>
        /// Initialize a model with a name.
        /// </summary>
        /// <param name="theName">Model name.</param>
        public ModelModel(ModelName theName)
            : this()
        {
            Name = theName;
        }

        /// <summary>
        /// Initialize a model model with default values.
        /// </summary>
        public ModelModel()
        {
            Name = new ModelName();
            Variables = new ObservableCollection<VariableModel>();
            Singletons = new ObservableCollection<SingletonVariableModel>();
            Aggregates = new ObservableCollection<AggregateVariableModel>();
            SharedDomains = new ObservableCollection<SharedDomainModel>();
            Constraints = new ObservableCollection<ConstraintModel>();
            Bundles = new ObservableCollection<BundleModel>();
            Buckets = new ObservableCollection<BucketModel>();
        }

        /// <summary>
        /// Gets and sets the workspace the model belongs.
        /// </summary>
        public WorkspaceModel Workspace
        {
            get { return this.workspace; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.workspace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the variables.
        /// </summary>
        public ObservableCollection<VariableModel> Variables
        {
            get { return this.variables; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.variables = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the singleton variable collection.
        /// </summary>
        public ObservableCollection<SingletonVariableModel> Singletons
        {
            get { return this.singletons; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.singletons = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the aggregate variables.
        /// </summary>
        public ObservableCollection<AggregateVariableModel> Aggregates
        {
            get { return this.aggregates; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.aggregates = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the domains.
        /// </summary>
        public ObservableCollection<SharedDomainModel> SharedDomains
        {
            get { return this._sharedDomains; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this._sharedDomains = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        public ObservableCollection<ConstraintModel> Constraints
        {
            get { return this.constraints; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.constraints = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the bundle collection.
        /// </summary>
        public ObservableCollection<BundleModel> Bundles
        {
            get { return this.bundles; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.bundles = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the buckets collection.
        /// </summary>
        public ObservableCollection<BucketModel> Buckets
        {
            get => _buckets;
            set
            {
                _buckets = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets whether the model has anything to solve.
        /// </summary>
        public bool IsEmpty => Variables.Count == 0 && Buckets.Count == 0;

        /// <summary>
        /// Add a new constraint to the model.
        /// </summary>
        /// <param name="newConstraint">New constraint.</param>
        public void AddConstraint(ConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            if (!newConstraint.HasIdentity)
            {
                newConstraint.AssignIdentity();
            }

            Constraints.Add(newConstraint);
        }

        /// <summary>
        /// Delete the constraint from the model.
        /// </summary>
        /// <param name="constraintToDelete">Constraint to delete.</param>
        public void DeleteConstraint(ConstraintModel constraintToDelete)
        {
            Contract.Requires<ArgumentNullException>(constraintToDelete != null);

            Constraints.Remove(constraintToDelete);
        }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        public void AddVariable(SingletonVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            if (!newVariable.HasIdentity)
            {
                newVariable.AssignIdentity();
            }

            Variables.Add(newVariable);
            Singletons.Add(newVariable);
            newVariable.Workspace = Workspace;
        }

        /// <summary>
        /// Add a new aggregate variable to the model.
        /// </summary>
        /// <param name="newVariable">New aggregate variable.</param>
        public void AddVariable(AggregateVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            if (!newVariable.HasIdentity)
            {
                newVariable.AssignIdentity();
            }

            Variables.Add(newVariable);
            Aggregates.Add(newVariable);
            newVariable.Workspace = Workspace;
        }

        /// <summary>
        /// Delete the variable from the model.
        /// </summary>
        /// <param name="variableToDelete">Variable to delete.</param>
        public void DeleteVariable(VariableModel variableToDelete)
        {
            Contract.Requires<ArgumentNullException>(variableToDelete != null);

            Variables.Remove(variableToDelete);
        }

        /// <summary>
        /// Add a shared domain to the model.
        /// </summary>
        /// <param name="newDomain">New shared domain.</param>
        public void AddSharedDomain(SharedDomainModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);
            Contract.Assume(newDomain.Name != null);

            if (!newDomain.HasIdentity)
            {
                newDomain.AssignIdentity();
            }
            SharedDomains.Add(newDomain);
        }

        /// <summary>
        /// Add a new bundle to the model.
        /// </summary>
        /// <param name="newBundle">New bundle.</param>
        public void AddBundle(BundleModel newBundle)
        {
            Contract.Requires<ArgumentNullException>(newBundle != null);
            Contract.Assume(newBundle.Name != null);
            if (!newBundle.HasIdentity)
            {
                newBundle.AssignIdentity();
            }

            Bundles.Add(newBundle);
        }

        /// <summary>
        /// Add a new bucket to the model.
        /// </summary>
        /// <param name="bucket">New bucket.</param>
        public void AddBucket(BucketModel bucket)
        {
            Contract.Requires<ArgumentNullException>(bucket != null);
            Contract.Assume(bucket.Name != null);
            if (!bucket.HasIdentity)
            {
                bucket.AssignIdentity();
            }

            Buckets.Add(bucket);
        }

        /// <summary>
        /// Delete the domain from the model.
        /// </summary>
        /// <param name="domainToDelete">Domain to delete.</param>
        public void DeleteDomain(SharedDomainModel domainToDelete)
        {
            Contract.Requires<ArgumentNullException>(domainToDelete != null);

            SharedDomains.Remove(domainToDelete);
        }

        /// <summary>
        /// Get the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">The variable name.</param>
        /// <returns>Variable model.</returns>
        public VariableModel GetVariableByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));

            return Variables.FirstOrDefault(variable => variable.Name.IsEqualTo(theVariableName));
        }

        /// <summary>
        /// Get the shared domain matching the given name.
        /// </summary>
        /// <param name="theSharedDomainName">Shared domain name.</param>
        /// <returns>Shared domain matching the name.</returns>
        public SharedDomainModel GetSharedDomainByName(string theSharedDomainName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theSharedDomainName));
            return SharedDomains.FirstOrDefault(sharedDomain => sharedDomain.Name.IsEqualTo(theSharedDomainName));
        }

        /// <summary>
        /// Get the bundle with the matching bundle name.
        /// </summary>
        /// <param name="bundleName">Name of the bundle to find.</param>
        /// <returns>Bundle matching the name.</returns>
        public BundleModel GetBundleByName(string bundleName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(bundleName));
            return Bundles.FirstOrDefault(bundle => bundle.Name.IsEqualTo(bundleName));
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        /// <returns>Solve result.</returns>
        public SolveResult Solve()
        {
            Contract.Ensures(Contract.Result<SolveResult>() != null);
            using (var solver = new OrToolsSolver())
            {
                return solver.Solve(this);
            }
        }
    }
}
