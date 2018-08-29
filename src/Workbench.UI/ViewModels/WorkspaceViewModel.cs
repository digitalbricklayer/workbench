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
    /// View model for the aplication work area.
    /// </summary>
    public sealed class WorkspaceViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private bool isDirty;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;
        private ModelEditorTabViewModel _modelEditor;
        private readonly IViewModelFactory _viewModelFactory;
        private SolutionViewerTabViewModel _solutionViewer;

        /// <summary>
        /// Initialize a work area view model with a data service, window manager and event aggregator.
        /// </summary>
        public WorkspaceViewModel(IDataService theDataService,
                                 IWindowManager theWindowManager,
                                 IEventAggregator theEventAggregator,
                                 IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this._eventAggregator = theEventAggregator;
            this._windowManager = theWindowManager;
            _viewModelFactory = theViewModelFactory;

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
            get
            {
                return this._modelEditor;
            }
            set
            {
                this._modelEditor = value;
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
        /// Gets or sets the work area dirty flag.
        /// </summary>
        public bool IsDirty
        {
            get { return this.isDirty; }
            set
            {
                if (this.isDirty == value) return;
                this.isDirty = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Solve the constraint model.
        /// </summary>
        public SolveResult SolveModel()
        {
            var isModelValid = Validate();
            if (!isModelValid) return SolveResult.InvalidModel;
            var solveResult = WorkspaceModel.Solve();
            if (!solveResult.IsSuccess) return SolveResult.Failed;
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
            IsDirty = true;
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
            IsDirty = true;
            _eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newVariable));
        }

        /// <summary>
        /// Create a new domain at a specific location.
        /// </summary>
        /// <param name="newDomain">New domain.</param>
        public void AddDomain(DomainModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);

            ModelEditor.AddDomain(newDomain);
            IsDirty = true;
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraint">New constraint name.</param>
        public void AddExpressionConstraint(ExpressionConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            ModelEditor.AddConstraint(newConstraint);
            IsDirty = true;
        }

        /// <summary>
        /// Create a new all different constraint.
        /// </summary>
        /// <param name="newConstraint">New all different constraint.</param>
        public void AddAllDifferentConstraint(AllDifferentConstraintModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            ModelEditor.AddConstraint(newConstraint);
            IsDirty = true;
        }

        /// <summary>
        /// Add a new chessboard tab to the workspace.
        /// </summary>
        /// <param name="newChessboard">New chessboard tab.</param>
        public void AddChessboardTab(ChessboardModel newChessboard)
        {
            Contract.Requires<ArgumentNullException>(newChessboard != null);
            var newChessboardTab = new ChessboardTabViewModel(new ChessboardTabModel(newChessboard, new WorkspaceTabTitle("board1")), _windowManager);
            ChessboardTabs.Add(newChessboardTab);
            ActivateItem(newChessboardTab);
            IsDirty = true;
        }

        /// <summary>
        /// Add a new table tab to the workspace.
        /// </summary>
        /// <param name="newTable">New table.</param>
        public void AddTableTab(TableModel newTable)
        {
            Contract.Requires<ArgumentNullException>(newTable != null);

            var newTableTab = new TableTabViewModel(new TableTabModel(TableModel.Default, new WorkspaceTabTitle("Table")), _eventAggregator, _windowManager);
            TableTabs.Add(newTableTab);
            ActivateItem(newTableTab);
            IsDirty = true;
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

        /// <summary>
        /// Solve the model.
        /// </summary>
        private bool Validate()
        {
            var validationContext = new ModelValidationContext();
            var isModelValid = WorkspaceModel.Model.Validate(validationContext);
            if (isModelValid) return true;
            Contract.Assume(validationContext.HasErrors);
            DisplayErrorDialog(validationContext);

            return false;
        }

        /// <summary>
        /// Display a dialog box with a display of all of the model errors.
        /// </summary>
        /// <param name="theContext">Validation context.</param>
        private void DisplayErrorDialog(ModelValidationContext theContext)
        {
            var errorsViewModel = CreateModelErrorsFrom(theContext);
            _windowManager.ShowDialog(errorsViewModel);
        }

        /// <summary>
        /// Create a model errros view model from a model.
        /// </summary>
        /// <param name="theContext">Validation context.</param>
        /// <returns>View model with all errors in the model.</returns>
        private static ModelErrorsViewModel CreateModelErrorsFrom(ModelValidationContext theContext)
        {
            Contract.Requires<ArgumentNullException>(theContext != null);
            Contract.Requires<InvalidOperationException>(theContext.HasErrors);

            var errorsViewModel = new ModelErrorsViewModel();
            foreach (var error in theContext.Errors)
            {
                var errorViewModel = new ModelErrorViewModel
                {
                    Message = error
                };
                errorsViewModel.Errors.Add(errorViewModel);
            }

            return errorsViewModel;
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
