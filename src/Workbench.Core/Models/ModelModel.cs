using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A model for specifying the problem.
    /// <remarks>Just a very simple finite integer domain at the moment.</remarks>
    /// </summary>
    [Serializable]
    public sealed class ModelModel : BundleModel
    {
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
        /// Initialize a model with default values.
        /// </summary>
        public ModelModel()
        {
            Name = new ModelName();
            Variables = new ObservableCollection<VariableModel>();
            Singletons = new ObservableCollection<SingletonVariableModel>();
            Aggregates = new ObservableCollection<AggregateVariableModel>();
            SharedDomains = new ObservableCollection<SharedDomainModel>();
            Constraints = new ObservableCollection<ConstraintModel>();
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
