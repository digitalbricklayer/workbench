using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Core.Solver;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the application work area.
    /// </summary>
    public sealed class WorkspaceViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private ModelEditorTabViewModel _modelEditor;
        private readonly IViewModelFactory _viewModelFactory;
        private SolutionViewerTabViewModel _solutionViewer;
        private readonly ModelValidatorViewModel _modelValidator;

        /// <summary>
        /// Initialize a work area view model with a data service, window manager and event aggregator.
        /// </summary>
        public WorkspaceViewModel(IDataService theDataService,
                                  IWindowManager theWindowManager,
                                  IEventAggregator theEventAggregator,
                                  IViewModelFactory theViewModelFactory,
                                  ModelValidatorViewModel theModelValidatorViewModel)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);
            Contract.Requires<ArgumentNullException>(theModelValidatorViewModel != null);

            _eventAggregator = theEventAggregator;
            _windowManager = theWindowManager;
            _viewModelFactory = theViewModelFactory;
            _modelValidator = theModelValidatorViewModel;

            WorkspaceModel = theDataService.GetWorkspace();
            Solution = WorkspaceModel.Solution;
            ChessboardTabs = new BindableCollection<ChessboardTabViewModel>();
            TableTabs = new BindableCollection<TableTabViewModel>();
        }

        /// <summary>
        /// Gets or sets the workspace model.
        /// </summary>
        public WorkspaceModel WorkspaceModel { get; }

        /// <summary>
        /// Gets or sets the model editor.
        /// </summary>
        public ModelEditorTabViewModel ModelEditor
        {
            get => _modelEditor;
            set
            {
                _modelEditor = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution viewer.
        /// </summary>
        public SolutionViewerTabViewModel SolutionViewer
        {
            get => _solutionViewer;
            set
            {
                _solutionViewer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets all chessboard visualizers.
        /// </summary>
        public BindableCollection<ChessboardTabViewModel> ChessboardTabs { get; }

        /// <summary>
        /// Gets all table visualizers.
        /// </summary>
        public BindableCollection<TableTabViewModel> TableTabs { get; }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Solution { get; }

        /// <summary>
        /// Solve the constraint model.
        /// </summary>
        public SolveResult SolveModel()
        {
            var isModelValid = _modelValidator.Validate();
            if (!isModelValid) return SolveResult.InvalidModel;
            var solveResult = WorkspaceModel.Solve();
            if (!solveResult.IsSuccess) return SolveResult.Failed;
            foreach (var anItem in TableTabs)
            {
                anItem.UpdateFromModel();
            }
            DisplaySolution(WorkspaceModel.Solution);
            _eventAggregator.PublishOnUIThread(new ModelSolvedMessage(solveResult));

            return solveResult;
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        /// <returns>New singleton variable view model.</returns>
        public void AddSingletonVariable(SingletonVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            ModelEditor.AddSingletonVariable(newVariable);
            _eventAggregator.PublishOnUIThread(new SingletonVariableAddedMessage(newVariable));
        }

        /// <summary>
        /// Create a new aggregate variable.
        /// </summary>
        /// <param name="newVariable">New variable name.</param>
        /// <returns>New aggregate variable view model.</returns>
        public void AddAggregateVariable(AggregateVariableModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            ModelEditor.AddAggregateVariable(newVariable);
            _eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newVariable));
        }

        /// <summary>
        /// Create a new domain at a specific location.
        /// </summary>
        /// <param name="newDomain">New domain.</param>
        public void AddDomain(SharedDomainModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);

            ModelEditor.AddDomain(newDomain);
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraint">New constraint name.</param>
        public void AddExpressionConstraint(ExpressionConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            ModelEditor.AddConstraint(newConstraint);
        }

        /// <summary>
        /// Create a new all different constraint.
        /// </summary>
        /// <param name="newConstraint">New all different constraint.</param>
        public void AddAllDifferentConstraint(AllDifferentConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            ModelEditor.AddConstraint(newConstraint);
        }

        /// <summary>
        /// Add a new chessboard tab to the workspace.
        /// </summary>
        /// <param name="newChessboard">New chessboard tab.</param>
        public void AddChessboardTab(ChessboardModel newChessboard)
        {
            Contract.Requires<ArgumentNullException>(newChessboard != null);
            var newChessboardTab = new ChessboardTabViewModel(new ChessboardTabModel(newChessboard, new WorkspaceTabTitle("board1")), _windowManager);
            WorkspaceModel.Display.AddVisualizer(newChessboardTab.Model);
            ChessboardTabs.Add(newChessboardTab);
            ActivateItem(newChessboardTab);
        }

        /// <summary>
        /// Add a new table tab to the workspace.
        /// </summary>
        /// <param name="newTable">New table.</param>
        public void AddTableTab(TableModel newTable)
        {
            Contract.Requires<ArgumentNullException>(newTable != null);

            var newTabDetailsViewModel = new NewTabDetailsViewModel();
            var result = _windowManager.ShowDialog(newTabDetailsViewModel);
            if (!result.GetValueOrDefault()) return;
            var tableModel = TableModel.Default;
            tableModel.Name = new ModelName(newTabDetailsViewModel.TabName);
            var newTableTab = new TableTabViewModel(new TableTabModel(tableModel,
                                                    new WorkspaceTabTitle(newTabDetailsViewModel.TabDescription)),
                                                    _eventAggregator,
                                                    _windowManager);
            WorkspaceModel.Display.AddVisualizer(newTableTab.Model);
            TableTabs.Add(newTableTab);
            ActivateItem(newTableTab);
        }

        public void BindTo(SolutionModel theSolution)
        {
        }

        /// <summary>
        /// Close the tab initiated by the user.
        /// </summary>
        public void CloseTab()
        {
            DeactivateItem(ActiveItem, true);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            // The model editor is always present in the workspace
            ModelEditor = _viewModelFactory.CreateModelEditor();
            Items.Add(ModelEditor);
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(SolutionModel theSolution)
        {
            BindTo(theSolution);
            /*
             * There is only ever one solution viewer, so re-use the same
             * view model if it already exists.
             */
            if (SolutionViewer == null)
            {
                SolutionViewer = new SolutionViewerTabViewModel();
            }

            ActivateItem(SolutionViewer);
            SolutionViewer.BindTo(theSolution);
        }

        [ContractInvariantMethod]
        private void WorkspaceInvariants()
        {
            Contract.Invariant(WorkspaceModel != null);
            Contract.Invariant(TableTabs != null);
            Contract.Invariant(ChessboardTabs != null);
        }
    }
}
