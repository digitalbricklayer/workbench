using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A bundle is a collection of variables.
    /// </summary>
    public sealed class BundleModel : Model
    {
        private ObservableCollection<SingletonVariableModel> _singletons;
        private ObservableCollection<AllDifferentConstraintModel> _allDifferentConstraints;

        /// <summary>
        /// Initialize a bundle with a name.
        /// </summary>
        /// <param name="bundleName">Bundle name.</param>
        public BundleModel(ModelName bundleName)
            : base(bundleName)
        {
            Singletons = new ObservableCollection<SingletonVariableModel>();
            AllDifferentConstraints = new ObservableCollection<AllDifferentConstraintModel>();
        }

        /// <summary>
        /// Gets the singleton variables contained inside the bundle.
        /// </summary>
        public ObservableCollection<SingletonVariableModel> Singletons
        {
            get => _singletons;
            private set
            {
                _singletons = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the all different constraints contained inside the bundle.
        /// </summary>
        public ObservableCollection<AllDifferentConstraintModel> AllDifferentConstraints
        {
            get => _allDifferentConstraints;
            private set
            {
                _allDifferentConstraints = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Add a singleton variable to the bundle.
        /// </summary>
        /// <param name="singletonVariable">Singleton variable to add.</param>
        public void AddSingleton(SingletonVariableModel singletonVariable)
        {
            Contract.Requires<ArgumentNullException>(singletonVariable != null);
            Singletons.Add(singletonVariable);
        }

        /// <summary>
        /// Add an all different constraint to the bundle.
        /// </summary>
        /// <param name="allDifferentConstraint"></param>
        public void AddAllDifferentConstraint(AllDifferentConstraintModel allDifferentConstraint)
        {
            Contract.Requires<ArgumentNullException>(allDifferentConstraint != null);
            AllDifferentConstraints.Add(allDifferentConstraint);
        }
    }
}
