using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the 'Model' menu.
    /// </summary>
    public class ModelMenuViewModel
    {
        private readonly IWindowManager windowManager;
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;
        private readonly IDataService dataService;

        public ModelMenuViewModel(IWindowManager theWindowManager,
                                  IAppRuntime theAppRuntime,
                                  TitleBarViewModel theTitleBarViewModel,
                                  IEventAggregator theEventAggregator,
                                  IViewModelService theViewModelService,
                                  IDataService theDataService)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.windowManager = theWindowManager;
            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;
            this.eventAggregator = theEventAggregator;
            this.viewModelService = theViewModelService;
            this.dataService = theDataService;

            SolveCommand = new CommandHandler(ModelSolveAction);
            AddSingletonVariableCommand = new CommandHandler(ModelAddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(ModelAddAggregateVariableAction);
            AddExpressionConstraintCommand = new CommandHandler(ModelAddExpressionConstraintAction);
            AddAllDifferentConstraintCommand = new CommandHandler(ModelAddAllDifferentConstraintAction);
            AddDomainCommand = new CommandHandler(ModelAddDomainAction);
            ResizeCommand = new CommandHandler(ModelResizeAction, _ => CanResizeExecute);
        }

        /// <summary>
        /// Gets the Model|Solve command
        /// </summary>
        public ICommand SolveCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Singleton Variable command.
        /// </summary>
        public ICommand AddSingletonVariableCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Aggregate Variable command.
        /// </summary>
        public ICommand AddAggregateVariableCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Expression Constraint command.
        /// </summary>
        public ICommand AddExpressionConstraintCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add All Different Constraint command.
        /// </summary>
        public ICommand AddAllDifferentConstraintCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Add Domain command.
        /// </summary>
        public ICommand AddDomainCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Resize command.
        /// </summary>
        public ICommand ResizeCommand { get; private set; }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkAreaViewModel WorkArea
        {
            get { return this.appRuntime.WorkArea; }
        }

        /// <summary>
        /// Gets whether the "Model|Resize" menu item can be executed.
        /// </summary>
        public bool CanResizeExecute
        {
            get
            {
                return WorkArea.Editor.GetSelectedAggregateVariables().Any();
            }
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        private void ModelSolveAction()
        {
            this.WorkArea.SolveModel();
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void ModelAddSingletonVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.WorkArea.AddSingletonVariable("New Variable", newVariableLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void ModelAddAggregateVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            var newAggregate = new AggregateVariableBuilder().WithName("New Variable")
                                                             .WithModel(WorkArea.WorkspaceModel.Model)
                                                             .WithDataService(this.dataService)
                                                             .WithEventAggregator(this.eventAggregator)
                                                             .WithViewModelService(this.viewModelService)
                                                             .Build();
            this.WorkArea.AddAggregateVariable(newAggregate, newVariableLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void ModelAddExpressionConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            WorkArea.AddExpressionConstraint(new ExpressionConstraintViewModel(new ExpressionConstraintGraphicModel(new ExpressionConstraintModel(new ModelName("New Constraint")))), newConstraintLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void ModelAddAllDifferentConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            WorkArea.AddAllDifferentConstraint("New Constraint", newConstraintLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        private void ModelAddDomainAction()
        {
            var newDomainLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.WorkArea.AddDomain("New Domain", newDomainLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Resize the selected aggregate variable.
        /// </summary>
        private void ModelResizeAction()
        {
            var selectedVariables = WorkArea.Editor.GetSelectedAggregateVariables();
            if (selectedVariables == null) return;

            var resizeViewModel = new AggregateVariableResizeViewModel();
            var showDialogResult = this.windowManager.ShowDialog(resizeViewModel);

            if (showDialogResult.GetValueOrDefault())
            {
                foreach (var variableViewModel in selectedVariables)
                {
                    var aggregate = (AggregateVariableEditorViewModel)variableViewModel;
                    aggregate.NumberVariables = Convert.ToString(resizeViewModel.Size);
                }
            }
        }
    }
}