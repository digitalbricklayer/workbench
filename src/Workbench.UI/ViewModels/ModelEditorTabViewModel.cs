using System.Diagnostics;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Properties;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the model editor tab.
    /// </summary>
    public sealed class ModelEditorTabViewModel : Conductor<BundleEditorViewModel>.Collection.OneActive,
                                                  IWorkspaceTabViewModel,
                                                  IHandle<BundleAddedMessage>,
                                                  IHandle<BundleDeletedMessage>,
                                                  IHandle<BundleRenamedMessage>
    {
        private readonly BundleEditorViewModel _rootBundleEditor;
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private IObservableCollection<string> _bundleNames = new BindableCollection<string>();
        private string _selectedBundleName;

        /// <summary>
        /// Initialize a model editor view model with the data service, event aggregator and window manager.
        /// </summary>
        public ModelEditorTabViewModel(IDataService theDataService, IEventAggregator theEventAggregator, IWindowManager theWindowManager)
        {
            _dataService = theDataService;
            _eventAggregator = theEventAggregator;
            _windowManager = theWindowManager;
            ModelModel = _dataService.GetWorkspace().Model;
            _rootBundleEditor = new BundleEditorViewModel(ModelModel, theDataService, theWindowManager, theEventAggregator);
            ActivateItem(_rootBundleEditor);
            SelectedBundleName = Resources.ModelRootName;
            BundleNames.Add(Resources.ModelRootName);
            _eventAggregator.Subscribe(this);
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
        public IObservableCollection<BundleModelItemViewModel> Bundles => ActiveItem.Bundles;

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
                Debug.Assert(!string.IsNullOrWhiteSpace(value));

                _selectedBundleName = value;
                if (_selectedBundleName == Resources.ModelRootName)
                {
                    ChangeActiveItem(_rootBundleEditor, closePrevious:false);
                }
                else
                {
                    var existingBundleEditor = FindBundleEditor(_selectedBundleName);
                    if (existingBundleEditor != null)
                    {
                        ChangeActiveItem(existingBundleEditor, closePrevious: false);
                    }
                    else
                    {
                        var bundleModel = ModelModel.GetBundleByName(_selectedBundleName);
                        var newBundleEditor = new BundleEditorViewModel(bundleModel, _dataService, _windowManager, _eventAggregator);
                        ChangeActiveItem(newBundleEditor, closePrevious: false);
                    }
                }
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the available bundle names.
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

        public void EditBundle(BundleModel bundleToEdit)
        {
            var newBundleEditor = new BundleEditorViewModel(bundleToEdit, _dataService, _windowManager, _eventAggregator);
            ActivateItem(newBundleEditor);
        }

        public void Handle(BundleAddedMessage message)
        {
            BundleNames.Add(message.BundleAdded.Name);
        }

        public void Handle(BundleRenamedMessage message)
        {
            var nameIndex = BundleNames.IndexOf(message.OldName);
            BundleNames[nameIndex] = message.BundleRenamed.Name.Text;
        }

        public void Handle(BundleDeletedMessage message)
        {
            BundleNames.Remove(message.BundleDeleted.Name);
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

        /// <summary>
        /// Find the existing bundle editor matching the bundle name.
        /// </summary>
        /// <param name="bundleName">Bundle name.</param>
        /// <returns>Bundle editor.</returns>
        private BundleEditorViewModel FindBundleEditor(string bundleName)
        {
            return Items.FirstOrDefault(editor => editor.Bundle.Name == bundleName);
        }
    }
}
