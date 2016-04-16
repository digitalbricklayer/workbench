using System;
using System.Collections.Generic;
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
        private readonly IObservableCollection<string> availableDisplayModes;
        private string selectedDisplayMode;
        private bool isDirty;
        private SolutionViewerViewModel viewer;
        private ModelViewModel model;
        private SolutionDesignerViewModel designer;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;
        private readonly IViewModelFactory viewModelFactory;

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
            this.availableDisplayModes = new BindableCollection<string> {"Model", "Designer"};
            this.WorkspaceModel = theDataService.GetWorkspace();
            this.model = this.viewModelFactory.CreateModel(this.WorkspaceModel.Model);
            this.viewer = new SolutionViewerViewModel(this.WorkspaceModel.Solution);
            this.designer = new SolutionDesignerViewModel(this.WorkspaceModel.Solution.Display);
            this.SelectedDisplayMode = "Model";
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
            get { return model; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.model = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution viewer.
        /// </summary>
        public SolutionViewerViewModel Viewer
        {
            get { return this.viewer; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.viewer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the solution designer.
        /// </summary>
        public SolutionDesignerViewModel Designer
        {
            get { return this.designer; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.designer = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected display mode.
        /// </summary>
        public string SelectedDisplayMode
        {
            get
            {
                return selectedDisplayMode;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.selectedDisplayMode = value;
                switch (this.selectedDisplayMode)
                {
                    case "Model":
                        this.ChangeActiveItem(this.Model, closePrevious:false);
                        break;

                    case "Solution":
                        this.ChangeActiveItem(this.Viewer, closePrevious:false);
                        break;

                    case "Designer":
                        this.ChangeActiveItem(this.Designer, closePrevious: false);
                        break;

                    default:
                        throw new NotImplementedException("Unknown display mode.");
                }
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the available display modes. Changes depending upon 
        /// whether the model has a solution or not.
        /// </summary>
        public IObservableCollection<string> AvailableDisplayModes
        {
            get
            {
                return this.availableDisplayModes;
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
            var isModelValid = this.Model.Validate();
            if (!isModelValid) return SolveResult.InvalidModel;
            var solveResult = this.WorkspaceModel.Solve();
            if (!solveResult.IsSuccess) return SolveResult.Failed;
            this.DisplaySolution(this.WorkspaceModel.Solution);
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

            var newVariable = new VariableViewModel(new VariableModel(newVariableName, newVariableLocation, new VariableDomainExpressionModel()),
                                                    this.eventAggregator);
            this.Model.AddSingletonVariable(newVariable);
            this.viewModelService.CacheVariable(newVariable);
            this.IsDirty = true;

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

            var newVariable = new AggregateVariableViewModel(new AggregateVariableModel(newVariableName, newVariableLocation, 1, new VariableDomainExpressionModel()),
                                                             this.eventAggregator);
            this.Model.AddAggregateVariable(newVariable);
            this.viewModelService.CacheVariable(newVariable);
            this.IsDirty = true;

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

            var newDomain = new DomainViewModel(new DomainModel(newDomainName, newDomainLocation, new DomainExpressionModel()));
            this.Model.AddDomain(newDomain);
            this.IsDirty = true;

            return newDomain;
        }

        /// <summary>
        /// Create a new constraint.
        /// </summary>
        /// <param name="newConstraintName">New constraint name.</param>
        /// <param name="newLocation">New constraint location.</param>
        /// <returns>New constraint view model.</returns>
        public ConstraintViewModel AddConstraint(string newConstraintName, Point newLocation)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(newConstraintName));

            var newConstraint = new ConstraintViewModel(new ExpressionConstraintModel(newConstraintName, newLocation, new ConstraintExpressionModel()));
            this.Model.AddConstraint(newConstraint);
            this.IsDirty = true;

            return newConstraint;
        }

        /// <summary>
        /// Add a new designer to the solution designer.
        /// </summary>
        /// <param name="newDesigner">Visualizer designer.</param>
        public void AddDesigner(VisualizerDesignViewModel newDesigner)
        {
            Contract.Requires<ArgumentNullException>(newDesigner != null);

            this.Designer.AddVisualizer(newDesigner);
            this.IsDirty = true;
        }

        /// <summary>
        /// Add a new viewer to the solution designer.
        /// </summary>
        /// <param name="newViewer">Variable viewer.</param>
        public void AddViewer(VisualizerViewerViewModel newViewer)
        {
            Contract.Requires<ArgumentNullException>(newViewer != null);

            this.Viewer.AddVisualizer(newViewer);
            this.IsDirty = true;
        }

        /// <summary>
        /// Delete all selected graphic items.
        /// </summary>
        public void DeleteSelectedGraphics()
        {
            this.DeleteSelectedVariables();
            this.DeleteSelectedDomains();
            this.DeleteConstraints();
        }

        /// <summary>
        /// Delete the variable from the view-model.
        /// </summary>
        public void DeleteVariable(VariableViewModel variable)
        {
            Contract.Requires<ArgumentNullException>(variable != null);

            this.Model.DeleteVariable(variable);
            this.IsDirty = true;
        }

        /// <summary>
        /// Reset the contents of the workspace.
        /// </summary>
        public void Reset()
        {
            this.Model.Reset();
            this.Viewer.Reset();
            this.IsDirty = true;
        }

        /// <summary>
        /// Get the model.
        /// </summary>
        /// <returns>Model view model.</returns>
        public ModelViewModel GetModel()
        {
            Contract.Ensures(Contract.Result<ModelViewModel>() != null);
            return this.Model;
        }

        /// <summary>
        /// Display the solution.
        /// </summary>
        /// <param name="theSolution">A valid solution.</param>
        private void DisplaySolution(SolutionModel theSolution)
        {
            this.Viewer.UnbindAll();
            var newValues = new List<ValueModel>();
            newValues.AddRange(theSolution.SingletonValues);
            newValues.AddRange(theSolution.AggregateValues);
            this.Viewer.BindTo(newValues);

            if (!this.AvailableDisplayModes.Contains("Solution"))
                this.AvailableDisplayModes.Add("Solution");
            this.SelectedDisplayMode = "Solution";
        }

        /// <summary>
        /// Delete the currently selected variables from the view-model.
        /// </summary>
        private void DeleteSelectedVariables()
        {
            // Take a copy of the variables list so we can delete domains while iterating.
            var variablesCopy = this.Model.Variables.ToArray();

            foreach (var variable in variablesCopy)
            {
                if (variable.IsSelected)
                {
                    this.DeleteVariable(variable);
                }
            }
        }

        private void DeleteSelectedDomains()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var domainCopy = this.Model.Domains.ToArray();

            foreach (var domain in domainCopy)
            {
                if (domain.IsSelected)
                {
                    this.Model.DeleteDomain(domain);
                }
            }
        }

        private void DeleteConstraints()
        {
            // Take a copy of the domains list so we can delete domains while iterating.
            var constraintsCopy = this.Model.Constraints.ToArray();

            foreach (var constraint in constraintsCopy)
            {
                if (constraint.IsSelected)
                {
                    this.Model.DeleteConstraint(constraint);
                }
            }
        }

        [ContractInvariantMethod]
        private void WorkspaceInvariants()
        {
            Contract.Invariant(this.Model != null);
            Contract.Invariant(this.Designer != null);
        }
    }
}
