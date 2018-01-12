using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Core.Solver;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the workspace where a model can be edited and 
    /// the solution displayed.
    /// </summary>
    public sealed class WorkspaceViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IObservableCollection<string> availableDisplays;
        private string selectedDisplay;
        private bool isDirty;
        private ModelViewModel model;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;
        private readonly IViewModelFactory viewModelFactory;
        private SolutionViewModel solution;

        /// <summary>
        /// Initialize a workspace view model with a data service, window manager and event aggregator.
        /// </summary>
        public WorkspaceViewModel(IDataService theDataService,
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

            this.eventAggregator = theEventAggregator;
            this.viewModelService = theViewModelService;
            this.viewModelFactory = theViewModelFactory;
            this.availableDisplays = new BindableCollection<string> {"Designer", "Solution"};
            WorkspaceModel = theDataService.GetWorkspace();
            this.model = this.viewModelFactory.CreateModel(WorkspaceModel.Model);
            Solution = new SolutionViewModel(this,
                                             new SolutionDesignerViewModel(WorkspaceModel.Solution.Display), 
                                             new SolutionViewerViewModel(WorkspaceModel.Solution));
            SelectedDisplay = "Designer";
        }

        /// <summary>
        /// Gets or sets the workspace model.
        /// </summary>
        public WorkspaceModel WorkspaceModel { get; set; }

        /// <summary>
        /// Gets or sets the model displayed in the workspace.
        /// </summary>
        public ModelViewModel Model
        {
            get { return this.model; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.model = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the workspace solution.
        /// </summary>
        public SolutionViewModel Solution
        {
            get { return this.solution; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.solution = value;
                NotifyOfPropertyChange();
            }
        }

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
                    case "Solution":
                        ChangeActiveItem(Solution.Viewer, closePrevious:false);
                        break;

                    case "Designer":
                        ChangeActiveItem(Solution.Designer, closePrevious: false);
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
        /// Gets or sets the workspace dirty flag.
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
            var isModelValid = Model.Validate();
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
        /// <param name="newVariableName">New variable name.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New singleton variable view model.</returns>
        public VariableViewModel AddSingletonVariable(string newVariableName, Point newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newVariableName));

            var newVariable = new SingletonVariableViewModel(new SingletonVariableGraphicModel(Model.Model, newVariableName, newVariableLocation, new VariableDomainExpressionModel()),
                                                             this.eventAggregator);
            Model.AddSingletonVariable(newVariable);
            this.viewModelService.CacheVariable(newVariable);
            IsDirty = true;

            return newVariable;
        }

        /// <summary>
        /// Create a new aggregate variable.
        /// </summary>
        /// <param name="newVariableName">New variable name.</param>
        /// <param name="newVariableLocation">New variable location.</param>
        /// <returns>New aggregate variable view model.</returns>
        public VariableViewModel AddAggregateVariable(string newVariableName, Point newVariableLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newVariableName));

            var newVariable = new AggregateVariableViewModel(new AggregateVariableGraphicModel(Model.Model, newVariableName, newVariableLocation, 1, new VariableDomainExpressionModel()),
                                                             this.eventAggregator);
            Model.AddAggregateVariable(newVariable);
            this.viewModelService.CacheVariable(newVariable);
            IsDirty = true;

            return newVariable;
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        /// <param name="newDomainName">New domain name.</param>
        /// <param name="newDomainLocation">New domain location.</param>
        /// <returns>New domain view model.</returns>
        public DomainViewModel AddDomain(string newDomainName, Point newDomainLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newDomainName));

            var newDomain = new DomainViewModel(new DomainGraphicModel(newDomainName, newDomainLocation, new DomainModel()));
            Model.AddDomain(newDomain);
            IsDirty = true;

            return newDomain;
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        public ConstraintViewModel AddExpressionConstraint(string newConstraintName, Point newLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newConstraintName));

            var newConstraint = new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel(newConstraintName, newLocation, new ExpressionConstraintModel()));
            Model.AddConstraint(newConstraint);
            this.IsDirty = true;

            return newConstraint;
        }

        /// <summary>
        /// Create a new all different constraint.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        public ConstraintViewModel AddAllDifferentConstraint(string newConstraintName, Point newLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newConstraintName));

            var newConstraint = new AllDifferentConstraintViewModel(new AllDifferentConstraintGraphicModel(newConstraintName, newLocation));
            Model.AddConstraint(newConstraint);
            this.IsDirty = true;

            return newConstraint;
        }

        /// <summary>
        /// Add a new chessboard visualizer to the workspace.
        /// </summary>
        /// <param name="newVisualizer">New chessboard visualizer.</param>
        public void AddChessboardVisualizer(ChessboardVisualizerViewModel newVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newVisualizer != null);

            Solution.AddChessboardVisualizer(newVisualizer);
            IsDirty = true;
        }

        /// <summary>
        /// Add a new grid visualizer to the workspace.
        /// </summary>
        /// <param name="newGridVisualizer">New grid visualizer.</param>
        public void AddGridVisualizer(GridVisualizerViewModel newGridVisualizer)
        {
            Contract.Requires<ArgumentNullException>(newGridVisualizer != null);

            Solution.AddGridVisualizer(newGridVisualizer);
            IsDirty = true;
        }

        /// <summary>
        /// Delete all selected graphic items.
        /// </summary>
        public void DeleteSelectedGraphics()
        {
            DeleteSelectedVariables();
            DeleteSelectedDomains();
            DeleteConstraints();
        }

        /// <summary>
        /// Delete the variable from the view-model.
        /// </summary>
        public void DeleteVariable(VariableViewModel variable)
        {
            Contract.Requires<ArgumentNullException>(variable != null);

            Model.DeleteVariable(variable);
            IsDirty = true;
        }

        /// <summary>
        /// Reset the contents of the workspace.
        /// </summary>
        public void Reset()
        {
            Model.Reset();
            Solution.Reset();
            IsDirty = true;
        }

        /// <summary>
        /// Get the model.
        /// </summary>
        /// <returns>Model view model.</returns>
        public ModelViewModel GetModel()
        {
            Contract.Ensures(Contract.Result<ModelViewModel>() != null);
            return Model;
        }

        /// <summary>
        /// Change the selected display to the new selected display.
        /// </summary>
        /// <param name="newSelectedDisplayMode">Name of the new display mode.</param>
        public void ChangeSelectedDisplayTo(string newSelectedDisplayMode)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newSelectedDisplayMode));
            Contract.Requires<ArgumentOutOfRangeException>(AvailableDisplays.Contains(newSelectedDisplayMode));
            SelectedDisplay = newSelectedDisplayMode;
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(SolutionModel theSolution)
        {
            Solution.Reset();
            Solution.BindTo(theSolution);

            if (!AvailableDisplays.Contains("Solution"))
                AvailableDisplays.Add("Solution");
            SelectedDisplay = "Solution";
        }

        /// <summary>
        /// Delete the currently selected variables from the view-model.
        /// </summary>
        private void DeleteSelectedVariables()
        {
            // Take a copy of the variables list so we can delete domains while iterating.
            var variablesCopy = Model.Variables.ToArray();

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
            var domainCopy = Model.Domains.ToArray();

            foreach (var domain in domainCopy)
            {
                if (domain.IsSelected)
                {
                    Model.DeleteDomain(domain);
                }
            }
        }

        private void DeleteConstraints()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var constraintsCopy = Model.Constraints.ToArray();

            foreach (var constraint in constraintsCopy)
            {
                if (constraint.IsSelected)
                {
                    Model.DeleteConstraint(constraint);
                }
            }
        }

        [ContractInvariantMethod]
        private void WorkspaceInvariants()
        {
            Contract.Invariant(Model != null);
            Contract.Invariant(Solution != null);
        }
    }
}
