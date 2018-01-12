using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;

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

        public ModelMenuViewModel(IWindowManager theWindowManager,
                                  IAppRuntime theAppRuntime,
                                  TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            this.windowManager = theWindowManager;
            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;

            SolveCommand = new CommandHandler(ModelSolveAction);
            AddSingletonVariableCommand = new CommandHandler(ModelAddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(ModelAddAggregateVariableAction);
            AddExpressionConstraintCommand = new CommandHandler(ModelAddExpressionConstraintAction);
            AddAllDifferentConstraintCommand = new CommandHandler(ModelAddAllDifferentConstraintAction);
            AddDomainCommand = new CommandHandler(ModelAddDomainAction);
            DeleteCommand = new CommandHandler(ModelDeleteAction, _ => CanDeleteExecute);
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
        /// Gets the Model|Delete command.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets the Model|Resize command.
        /// </summary>
        public ICommand ResizeCommand { get; private set; }

        /// <summary>
        /// Gets the workspace view model.
        /// </summary>
        public WorkAreaViewModel Workspace
        {
            get { return this.appRuntime.Workspace; }
            set { this.appRuntime.Workspace = value; }
        }

        /// <summary>
        /// Gets whether the "Model|Delete" menu item can be executed.
        /// </summary>
        public bool CanDeleteExecute
        {
            get
            {
                return this.Workspace.Model.Items.Any(_ => _.IsSelected);
            }
        }

        /// <summary>
        /// Gets whether the "Model|Resize" menu item can be executed.
        /// </summary>
        public bool CanResizeExecute
        {
            get
            {
                return this.Workspace.Model.GetSelectedAggregateVariables().Any();
            }
        }

        /// <summary>
        /// Solve the model.
        /// </summary>
        private void ModelSolveAction()
        {
            this.Workspace.SolveModel();
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void ModelAddSingletonVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddSingletonVariable("New Variable", newVariableLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void ModelAddAggregateVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddAggregateVariable("New Variable", newVariableLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void ModelAddExpressionConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            Workspace.AddExpressionConstraint("New Constraint", newConstraintLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void ModelAddAllDifferentConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            Workspace.AddAllDifferentConstraint("New Constraint", newConstraintLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        private void ModelAddDomainAction()
        {
            var newDomainLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.Workspace.AddDomain("New Domain", newDomainLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Delete all selected graphics.
        /// </summary>
        private void ModelDeleteAction()
        {
            this.Workspace.DeleteSelectedGraphics();
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Resize the selected aggregate variable.
        /// </summary>
        private void ModelResizeAction()
        {
            var selectedVariables = this.Workspace.Model.GetSelectedAggregateVariables();
            if (selectedVariables == null) return;

            var resizeViewModel = new AggregateVariableResizeViewModel();
            var showDialogResult = this.windowManager.ShowDialog(resizeViewModel);

            if (showDialogResult.GetValueOrDefault())
            {
                foreach (var variableViewModel in selectedVariables)
                {
                    var aggregate = (AggregateVariableViewModel)variableViewModel;
                    aggregate.NumberVariables = Convert.ToString(resizeViewModel.Size);
                }
            }
        }
    }
}