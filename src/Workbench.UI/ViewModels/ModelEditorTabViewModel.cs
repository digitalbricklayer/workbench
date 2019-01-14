using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Properties;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the model editor tab.
    /// </summary>
    public sealed class ModelEditorTabViewModel : Conductor<BundleEditorViewModel>.Collection.OneActive, IWorkspaceTabViewModel
    {
        private readonly BundleEditorViewModel _rootBundleEditor;
        private readonly IDataService _dataService;
        private IObservableCollection<string> _bundleNames = new BindableCollection<string>();
        private string _selectedBundleName;

        /// <summary>
        /// Initialize a model editor view model with the data service, event aggregator and window manager.
        /// </summary>
        public ModelEditorTabViewModel(IDataService theDataService, IEventAggregator theEventAggregator, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            _dataService = theDataService;
            ModelModel = _dataService.GetWorkspace().Model;
            _rootBundleEditor = new BundleEditorViewModel(ModelModel, theDataService, theWindowManager, theEventAggregator);
            ActivateItem(_rootBundleEditor);
            Items.AddRange(Bundles);
            SelectedBundleName = "Root";
            BundleNames.Add("Root");
        }

        /// <summary>
        /// Gets or sets the model model.
        /// </summary>
        public ModelModel ModelModel { get; set; }

        /// <summary>
        /// Gets whether the currently selected tab be closed by the user.
        /// </summary>
        public bool CloseTabIsVisible => false;

        /// <summary>
        /// Gets the variables in the currently selected bundle.
        /// </summary>
        public IObservableCollection<VariableModelItemViewModel> Variables => ActiveItem.Variables;

        /// <summary>
        /// Gets the shared domains in the currently selected bundle.
        /// </summary>
        public IObservableCollection<SharedDomainModelItemViewModel> Domains => ActiveItem.Domains;

        /// <summary>
        /// Gets the constraints in the currently selected bundle.
        /// </summary>
        public IObservableCollection<ConstraintModelItemViewModel> Constraints => ActiveItem.Constraints;

        /// <summary>
        /// Gets the bundles in the currently selected bundle.
        /// </summary>
        public IObservableCollection<BundleEditorViewModel> Bundles => ActiveItem.Bundles;

        /// <summary>
        /// Gets or sets the tab text.
        /// </summary>
        public string TabText
        {
            get => DisplayName;
            set
            {
                DisplayName = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the name of the selected bundle.
        /// </summary>
        public string SelectedBundleName
        {
            get => _selectedBundleName;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _selectedBundleName = value;
                if (_selectedBundleName == "Root")
                {
                    ChangeActiveItem(_rootBundleEditor, closePrevious:false);
                }
                else
                {
                    var bundleEditor = Bundles.First(bundle => bundle.DisplayName == _selectedBundleName);
                    ChangeActiveItem(bundleEditor, closePrevious: false);
                }
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the available bundles.
        /// </summary>
        public IObservableCollection<string> BundleNames
        {
            get => _bundleNames;
            set
            {
                _bundleNames = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddAggregateVariable(AggregateVariableModel newVariable)
        {
            ActiveItem.AddAggregateVariable(newVariable);
        }

        public void AddSingletonVariable(SingletonVariableModel newVariable)
        {
            ActiveItem.AddSingletonVariable(newVariable);
        }

        public void AddDomain(SharedDomainModel newDomain)
        {
            ActiveItem.AddDomain(newDomain);
        }

        public void AddConstraint(ExpressionConstraintModel newConstraint)
        {
            ActiveItem.AddConstraint(newConstraint);
        }

        public void AddConstraint(AllDifferentConstraintModel newConstraint)
        {
            ActiveItem.AddConstraint(newConstraint);
        }

        public VariableModelItemViewModel GetVariableByName(string variableName)
        {
            return ActiveItem.GetVariableByName(variableName);
        }

        public void DeleteVariable(VariableModelItemViewModel variableToDelete)
        {
            ActiveItem.DeleteVariable(variableToDelete);
        }

        internal void FixupAggregateVariable(AggregateVariableModelItemViewModel variableViewModel)
        {
            ActiveItem.FixupAggregateVariable(variableViewModel);
        }

        internal void FixupConstraint(ConstraintModelItemViewModel constraintViewModel)
        {
            ActiveItem.FixupConstraint(constraintViewModel);
        }

        internal void FixupSingletonVariable(SingletonVariableModelItemViewModel variableViewModel)
        {
            ActiveItem.FixupSingletonVariable(variableViewModel);
        }

        internal void FixupDomain(SharedDomainModelItemViewModel domainViewModel)
        {
            ActiveItem.FixupDomain(domainViewModel);
        }

        protected override void OnInitialize()
        {
            DisplayName = Resources.ModelEditorTabName;
            base.OnInitialize();
        }
    }
}
