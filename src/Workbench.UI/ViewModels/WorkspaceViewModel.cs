using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Core.Solver;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the application workspace.
    /// </summary>
    public sealed class WorkspaceViewModel : Conductor<IWorkspaceTabViewModel>.Collection.OneActive, IWorkspace
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private ModelEditorTabViewModel _modelEditor;
        private readonly IViewModelFactory _viewModelFactory;
        private SolutionViewerTabViewModel _solutionViewer;
        private readonly ModelValidatorViewModel _modelValidator;

        /// <summary>
        /// Initialize a workspace view model with a data service, window manager and event aggregator.
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
            Bindings = new BindableCollection<VisualizerBindingExpressionViewModel>();
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
        /// Gets all chessboard tab visualizers.
        /// </summary>
        public BindableCollection<ChessboardTabViewModel> ChessboardTabs { get; }

        /// <summary>
        /// Gets all table tab visualizers.
        /// </summary>
        public BindableCollection<TableTabViewModel> TableTabs { get; }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Solution { get; }

        /// <summary>
        /// Gets the visualizer binding expressions.
        /// </summary>
        public BindableCollection<VisualizerBindingExpressionViewModel> Bindings { get; }

        /// <summary>
        /// Solve the constraint model.
        /// </summary>
        public SolveResult SolveModel()
        {
            var isModelValid = _modelValidator.Validate();
            if (!isModelValid) return SolveResult.InvalidModel;
            var solveResult = WorkspaceModel.Solve();
            if (!solveResult.IsSuccess) return SolveResult.Failed;
            UpdateVisualizers();
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
            ModelEditor.AddAggregateVariable(newVariable);
            _eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newVariable));
        }

        /// <summary>
        /// Create a new domain at a specific location.
        /// </summary>
        /// <param name="newDomain">New domain.</param>
        public void AddDomain(SharedDomainModel newDomain)
        {
            ModelEditor.AddDomain(newDomain);
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraint">New constraint name.</param>
        public void AddExpressionConstraint(ExpressionConstraintModel newConstraint)
        {
            ModelEditor.AddConstraint(newConstraint);
        }

        /// <summary>
        /// Create a new all different constraint.
        /// </summary>
        /// <param name="newConstraint">New all different constraint.</param>
        public void AddAllDifferentConstraint(AllDifferentConstraintModel newConstraint)
        {
            ModelEditor.AddConstraint(newConstraint);
        }

        /// <summary>
        /// Add a new chessboard tab to the workspace.
        /// </summary>
        /// <param name="newChessboard">New chessboard tab.</param>
        public void AddChessboardTab(ChessboardModel newChessboard)
        {
            var newChessboardTab = new ChessboardTabViewModel(new ChessboardTabModel(newChessboard, new WorkspaceTabTitle("board1")), _windowManager);
            WorkspaceModel.Display.AddVisualizer(newChessboardTab.Model);
            LoadChessboardTab(newChessboardTab);
        }

        /// <summary>
        /// Add a new table tab to the workspace.
        /// </summary>
        /// <param name="newTable">New table.</param>
        public void AddTableTab(TableModel newTable)
        {
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

        /// <summary>
        /// Close the tab as initiated by the user.
        /// </summary>
        public void CloseTab(IWorkspaceTabViewModel tabToClose)
        {
            Contract.Assert(tabToClose.CloseTabIsVisible, "Should not be asked to close tabs that are not closable");

            switch (tabToClose)
            {
                case ChessboardTabViewModel chessboardTab:
                    ChessboardTabs.Remove(chessboardTab);
                    break;

                case TableTabViewModel tableTab:
                    TableTabs.Remove(tableTab);
                    break;

                default:
                    throw new NotImplementedException();
            }
            DeactivateItem(tabToClose, true);
        }

        /// <summary>
        /// Add a new visualizer binding expression.
        /// </summary>
        /// <param name="aNewExpression">A new visualizer binding expression.</param>
        public void AddBindingExpression(VisualizerBindingExpressionModel aNewExpression)
        {
            WorkspaceModel.AddBindingExpression(aNewExpression);
            LoadBindingExpression(new VisualizerBindingExpressionViewModel(aNewExpression));
        }

        /// <summary>
        /// Delete a visualizer binding expression.
        /// </summary>
        /// <param name="aVisualizerBinding">Visualizer binding to delete.</param>
        public void DeleteBindingExpression(VisualizerBindingExpressionViewModel aVisualizerBinding)
        {
            WorkspaceModel.DeleteBindingExpression(aVisualizerBinding.VisualizerExpression);
            var editorToDelete = GetBindingExpressionById(aVisualizerBinding.Id);
            Contract.Assert(editorToDelete != null);
            Bindings.Remove(editorToDelete);
        }

        /// <summary>
        /// Load a binding expression into the view model.
        /// </summary>
        /// <param name="anExpression">A binding expression to add to the workspace may be a new binding or one loaded from the model.</param>
        internal void LoadBindingExpression(VisualizerBindingExpressionViewModel anExpression)
        {
            Contract.Requires<ArgumentNullException>(anExpression != null);
            Bindings.Add(anExpression);
        }

        /// <summary>
        /// Load a chessboard tab into the view model.
        /// </summary>
        /// <param name="aChessboardTab">A chessboard tab to add to the workspace may be a new chessboard or one loaded from the model.</param>
        internal void LoadChessboardTab(ChessboardTabViewModel aChessboardTab)
        {
            Contract.Requires<ArgumentNullException>(aChessboardTab != null);
            ChessboardTabs.Add(aChessboardTab);
            if (IsActive)
            {
                ActivateItem(aChessboardTab);
            }
        }

        /// <summary>
        /// Get a visualizer binding expression using the identity.
        /// </summary>
        /// <param name="bindingExpressionId">Visualizer binding expression identity.</param>
        /// <returns>Visualizer binding expression view model matching the identity.</returns>
        public VisualizerBindingExpressionViewModel GetBindingExpressionById(int bindingExpressionId)
        {
            return Bindings.FirstOrDefault(binding => binding.Id == bindingExpressionId);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            // The model editor is always present in the workspace
            ModelEditor = _viewModelFactory.CreateModelEditor();
            Items.Add(ModelEditor);
            foreach (var aChessboardTab in ChessboardTabs)
            {
                Items.Add(aChessboardTab);
            }
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(SolutionModel theSolution)
        {
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

        private void UpdateVisualizers()
        {
            UpdateTables();
            UpdateChessboards();
        }

        private void UpdateChessboards()
        {
            foreach (var aChessboardTab in ChessboardTabs)
            {
                aChessboardTab.UpdateFromModel();
            }
        }

        private void UpdateTables()
        {
            foreach (var aTableTab in TableTabs)
            {
                aTableTab.UpdateFromModel();
            }
        }

        [ContractInvariantMethod]
        private void WorkspaceInvariants()
        {
            Contract.Invariant(WorkspaceModel != null);
            Contract.Invariant(TableTabs != null);
            Contract.Invariant(ChessboardTabs != null);
            Contract.Invariant(Bindings != null);
        }
    }
}
