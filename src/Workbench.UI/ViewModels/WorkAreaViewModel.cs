using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Core.Solver;
using Workbench.Messages;
using Workbench.Services;
using System.Collections.Generic;
using System.Windows.Input;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the aplication work area.
    /// </summary>
    public sealed class WorkAreaViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IObservableCollection<string> availableDisplays;
        private string selectedDisplay;
        private bool isDirty;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;
        private readonly IViewModelFactory viewModelFactory;
        private readonly IWindowManager windowManager;
        private WorkspaceViewerPanelViewModel viewer;
        private WorkspaceEditorViewModel editor;
        private readonly IDataService dataService;

        /// <summary>
        /// Initialize a work area view model with a data service, window manager and event aggregator.
        /// </summary>
        public WorkAreaViewModel(IDataService theDataService,
                                 IWindowManager theWindowManager,
                                 IEventAggregator theEventAggregator,
                                 IViewModelService theViewModelService,
                                 IViewModelFactory theViewModelFactory)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);
            Contract.Requires<ArgumentNullException>(theViewModelFactory != null);

            this.dataService = theDataService;
            this.eventAggregator = theEventAggregator;
            this.viewModelService = theViewModelService;
            this.viewModelFactory = theViewModelFactory;
            this.windowManager = theWindowManager;
            this.availableDisplays = new BindableCollection<string> {"Editor", "Viewer"};
            WorkspaceModel = theDataService.GetWorkspace();
            Solution = WorkspaceModel.Solution;
            AllVisualizers = new BindableCollection<VisualizerViewModel>();
            VariableVisualizers = new BindableCollection<VariableVisualizerViewModel>();
            ChessboardVisualizers = new BindableCollection<ChessboardVisualizerViewModel>();
            TableVisualizers = new BindableCollection<TableVisualizerViewModel>();
            Editor = new WorkspaceEditorViewModel(WorkspaceModel.Solution.Display, WorkspaceModel.Model);
            Viewer = new WorkspaceViewerPanelViewModel(WorkspaceModel.Solution);
            DeleteCommand = new CommandHandler(DeleteAction);
            SelectedDisplay = "Editor";
        }

        /// <summary>
        /// Gets or sets the workspace model.
        /// </summary>
        public WorkspaceModel WorkspaceModel { get; set; }

        /// <summary>
        /// Gets the Delete command.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets or sets the workspace editor.
        /// </summary>
        public WorkspaceEditorViewModel Editor
        {
            get
            {
                return this.editor;
            }
            set
            {
                this.editor = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the workspace viewer.
        /// </summary>
        public WorkspaceViewerPanelViewModel Viewer
        {
            get
            {
                return this.viewer;
            }
            set
            {
                this.viewer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        public SolutionModel Solution { get; set; }

        /// <summary>
        /// Gets all visualizers.
        /// </summary>
        public BindableCollection<VisualizerViewModel> AllVisualizers { get; private set; }

        /// <summary>
        /// Gets all chessboard visualizers.
        /// </summary>
        public BindableCollection<VariableVisualizerViewModel> VariableVisualizers { get; private set; }

        /// <summary>
        /// Gets all chessboard visualizers.
        /// </summary>
        public BindableCollection<ChessboardVisualizerViewModel> ChessboardVisualizers { get; private set; }

        /// <summary>
        /// Gets all table visualizers.
        /// </summary>
        public BindableCollection<TableVisualizerViewModel> TableVisualizers { get; private set; }

        /// <summary>
        /// Gets or sets the currently selected display mode.
        /// </summary>
        public string SelectedDisplay
        {
            get
            {
                return this.selectedDisplay;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.selectedDisplay = value;
                switch (this.selectedDisplay)
                {
                    case "Viewer":
                        ChangeActiveItem(Viewer, closePrevious:false);
                        break;

                    case "Editor":
                        ChangeActiveItem(Editor, closePrevious: false);
                        break;

                    default:
                        throw new NotImplementedException("Unknown display mode.");
                }
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the available displays. Changes depending upon 
        /// whether the model has a solution or not.
        /// </summary>
        public IObservableCollection<string> AvailableDisplays
        {
            get
            {
                return this.availableDisplays;
            }
        }

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
            this.eventAggregator.PublishOnUIThread(new ModelSolvedMessage(solveResult));

            return solveResult;
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        /// <returns>New singleton variable view model.</returns>
        public void AddSingletonVariable(SingletonVariableVisualizerViewModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            AddSingletonVariable(newVariable, new Point());
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        /// <param name="newVariableName">New variable name.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New singleton variable view model.</returns>
        public void AddSingletonVariable(string newVariableName, Point newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newVariableName));
            AddSingletonVariable(new SingletonVariableBuilder().WithName(newVariableName)
                                                               .WithModel(WorkspaceModel.Model)
                                                               .WithDataService(this.dataService)
                                                               .WithEventAggregator(this.eventAggregator)
                                                               .WithViewModelService(this.viewModelService)
                                                               .Build(),
                                 newVariableLocation);
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New singleton variable view model.</returns>
        public void AddSingletonVariable(SingletonVariableVisualizerViewModel newVariable, Point newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            AddVisualizer(newVariable);
            VariableVisualizers.Add(newVariable);
            Editor.AddSingletonVariable(newVariable.SingletonEditor);
//            Viewer.AddSingletonVariable(newVariable);
//            this.viewModelService.CacheVariable(newVariable);
            IsDirty = true;
            this.eventAggregator.PublishOnUIThread(new SingletonVariableAddedMessage(newVariable));
        }

        /// <summary>
        /// Create a new aggregate variable.
        /// </summary>
        /// <param name="newVariable">New variable name.</param>
        /// <returns>New aggregate variable view model.</returns>
        public void AddAggregateVariable(AggregateVariableVisualizerViewModel newVariable)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            AddAggregateVariable(newVariable, new Point());
        }

        /// <summary>
        /// Create a new aggregate variable.
        /// </summary>
        /// <param name="newVariable">New variable name.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New aggregate variable view model.</returns>
        public void AddAggregateVariable(AggregateVariableVisualizerViewModel newVariable, Point newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(newVariable != null);

            AddVisualizer(newVariable);
            VariableVisualizers.Add(newVariable);
            Editor.AddAggregateVariable(newVariable.AggregateEditor);
//            this.viewModelService.CacheVariable(newVariable);
            IsDirty = true;
            this.eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newVariable));
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        /// <param name="newDomain">New domain.</param>
        public void AddDomain(DomainVisualizerViewModel newDomain)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);

            AddDomain(newDomain, new Point());
        }

        /// <summary>
        /// Create a new domain at a specific location.
        /// </summary>
        /// <param name="newDomain">New domain.</param>
        /// <param name="newDomainLocation">New domain location.</param>
        public void AddDomain(DomainVisualizerViewModel newDomain, Point newDomainLocation)
        {
            Contract.Requires<ArgumentNullException>(newDomain != null);

            AllVisualizers.Add(newDomain);
            Editor.AddDomain(newDomain.DomainEditor);
            Viewer.AddVisualizer(newDomain.DomainViewer);
            IsDirty = true;
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraint">New constraint.</param>
        public void AddExpressionConstraint(ExpressionConstraintVisualizerViewModel newConstraint)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            AddExpressionConstraint(newConstraint, new Point());
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraint">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        public void AddExpressionConstraint(ExpressionConstraintVisualizerViewModel newConstraint, Point newLocation)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            AllVisualizers.Add(newConstraint);
            Editor.AddConstraint(newConstraint.ExpressionEditor);
            Viewer.AddVisualizer(newConstraint.Viewer);
            IsDirty = true;
        }

        /// <summary>
        /// Create a new all different constraint.
        /// </summary>
        /// <param name="newConstraint">New all different constraint.</param>
        /// <param name="newLocation">New constraint location.</param>
        public void AddAllDifferentConstraint(AllDifferentConstraintVisualizerViewModel newConstraint, Point newLocation)
        {
            Contract.Requires<ArgumentNullException>(newConstraint != null);

            AllVisualizers.Add(newConstraint);
            Editor.AddConstraint(newConstraint.AllDifferentEditor);
            Viewer.AddVisualizer(newConstraint.Viewer);
            IsDirty = true;
        }

        /// <summary>
        /// Add a new chessboard visualizer to the workspace.
        /// </summary>
        /// <param name="newVisualizer">New chessboard visualizer.</param>
        public void AddChessboardVisualizer(ChessboardVisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            AddVisualizer(newVisualizer);
            ChessboardVisualizers.Add(newVisualizer);
            IsDirty = true;
        }

        /// <summary>
        /// Add a new table visualizer to the workspace.
        /// </summary>
        /// <param name="newVisualizer">New table visualizer.</param>
        public void AddTableVisualizer(TableVisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);
            AddVisualizer(newVisualizer);
            TableVisualizers.Add(newVisualizer);
            IsDirty = true;
        }

        /// <summary>
        /// Delete all selected graphics.
        /// </summary>
        public void DeleteSelectedGraphics()
        {
            /*
             * TODO: this is awful. need something that ties both the editor and 
             * viewer together so that both can be deleted easily. If I'm on the 
             * editor and move to the viewer I probably will expect the editor that 
             * is selected on the editor to be selected on the viewer as well.
             */
            if (SelectedDisplay == "Editor")
            {
                var selectedEditors = Editor.DeleteSelectedGraphics();
                var selectedModels = selectedEditors.Select(_ => _.Model.Model);
                foreach (var model in selectedModels)
                {
//                    Viewer.
                }
            }
        }

        /// <summary>
        /// Delete the variable from the view-model.
        /// </summary>
        public void DeleteVariable(VariableVisualizerViewModel variable)
        {
            Contract.Requires<ArgumentNullException>(variable != null);

            Editor.DeleteVariable(variable.VariableEditor);
            IsDirty = true;
            this.eventAggregator.PublishOnUIThread(new VariableDeletedMessage(variable));
        }

        /// <summary>
        /// Reset the contents of the workspace.
        /// </summary>
        public void Reset()
        {
            Editor.Reset();
//            Viewer.Reset();
            IsDirty = true;
        }

        /// <summary>
        /// Change the selected display to the new selected display.
        /// </summary>
        /// <param name="newSelectedDisplayMode">Text of the new display mode.</param>
        public void ChangeSelectedDisplayTo(string newSelectedDisplayMode)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newSelectedDisplayMode));
            Contract.Requires<ArgumentOutOfRangeException>(AvailableDisplays.Contains(newSelectedDisplayMode));
            SelectedDisplay = newSelectedDisplayMode;
        }

        public void BindTo(SolutionModel theSolution)
        {
            Viewer.BindTo(theSolution);
        }

        public bool CanDeleteSelectedExecute()
        {
            if (SelectedDisplay == "Editor")
            {
                return Editor.Items.Any(_ => _.IsSelected);
            }
            else
            {
                throw new NotImplementedException("Selection is not implemented for the viewer");
            }
        }

        /// <summary>
        /// Get all selected grid visualizers.
        /// </summary>
        /// <returns>Collection of selected grid visualizers.</returns>
        public IReadOnlyCollection<TableVisualizerViewModel> GetSelectedGridVisualizers()
        {
            if (SelectedDisplay == "Editor")
            {
                return TableVisualizers.Where(gridVisualizer => gridVisualizer.Editor.IsSelected)
                                      .ToList();
            }

            return TableVisualizers.Where(gridVisualizer => gridVisualizer.Viewer.IsSelected)
                                  .ToList();
        }

        /// <summary>
        /// Get the variable matching the variable name.
        /// </summary>
        /// <param name="variableName">Variable name.</param>
        /// <returns>Variable matching the name.</returns>
        public VariableVisualizerViewModel GetVariableByName(string variableName)
        {
            return VariableVisualizers.FirstOrDefault(variableVisualizer => variableVisualizer.Name == variableName);
        }

        /// <summary>
        /// Add a new visualizer to the solution.
        /// </summary>
        /// <param name="newVisualizer">New visualizer.</param>
        private void AddVisualizer(VisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);

            AllVisualizers.Add(newVisualizer);
            Editor.AddVisualizer(newVisualizer.Editor);
            Viewer.AddVisualizer(newVisualizer.Viewer);
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(SolutionModel theSolution)
        {
            Reset();
            BindTo(theSolution);
            SelectedDisplay = "Viewer";
        }

#if false
        /// <summary>
        /// Delete the currently selected variables from the view-model.
        /// </summary>
        private void DeleteSelectedVariables()
        {
            // Take a copy of the variables list so we can delete domains while iterating.
            var variablesCopy = Editor.Variables.ToArray();

            foreach (var variable in variablesCopy)
            {
                if (variable.IsSelected)
                {
                    DeleteVariable(variable);
                }
            }
        }

        private void DeleteSelectedDomains()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var domainCopy = Editor.Domains.ToArray();

            foreach (var domain in domainCopy)
            {
                if (domain.IsSelected)
                {
                    Editor.DeleteDomain(domain);
                }
            }
        }

        private void DeleteConstraints()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var constraintsCopy = Editor.Constraints.ToArray();

            foreach (var constraint in constraintsCopy)
            {
                if (constraint.IsSelected)
                {
                    Editor.DeleteConstraint(constraint);
                }
            }
        }
#endif

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
            this.windowManager.ShowDialog(errorsViewModel);
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

        /// <summary>
        /// Delete all selected graphics.
        /// </summary>
        private void DeleteAction()
        {
            DeleteSelectedGraphics();
//            TitleBar.UpdateTitle();
        }

        [ContractInvariantMethod]
        private void WorkspaceInvariants()
        {
            Contract.Invariant(WorkspaceModel != null);
            Contract.Invariant(Solution != null);
            Contract.Invariant(Editor != null);
            Contract.Invariant(Viewer != null);
        }
    }
}
