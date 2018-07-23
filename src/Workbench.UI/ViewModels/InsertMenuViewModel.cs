using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class InsertMenuViewModel
    {
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;
        private readonly IWindowManager windowManager;

        public InsertMenuViewModel(IAppRuntime theAppRuntime,
                                   TitleBarViewModel theTitleBarViewModel,
                                   IEventAggregator theEventAggregator,
                                   IDataService theDataService,
                                   IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;
            this.windowManager = theWindowManager;

            AddSingletonVariableCommand = new CommandHandler(AddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(AddAggregateVariableAction);
            AddExpressionConstraintCommand = new CommandHandler(AddExpressionConstraintAction);
            AddAllDifferentConstraintCommand = new CommandHandler(AddAllDifferentConstraintAction);
            AddDomainCommand = new CommandHandler(AddDomainAction);
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkAreaViewModel WorkArea
        {
            get { return this.appRuntime.WorkArea; }
        }

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
        /// Create a new singleton variable.
        /// </summary>
        private void AddSingletonVariableAction()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditViewModel();
            var x = this.windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!x.HasValue) return;
            this.WorkArea.AddSingletonVariable(new SingletonVariableBuilder().WithName(singletonVariableEditorViewModel.VariableName)
                                                                             .WithDomain(singletonVariableEditorViewModel.DomainExpression)
                                                                             .WithModel(WorkArea.WorkspaceModel.Model)
                                                                             .Build());
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void AddAggregateVariableAction()
        {
            var newAggregate = new AggregateVariableBuilder().WithName("New Variable")
                                                             .WithModel(WorkArea.WorkspaceModel.Model)
                                                             .Build();
            this.WorkArea.AddAggregateVariable(newAggregate);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void AddExpressionConstraintAction()
        {
            WorkArea.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName("New Constraint")
                    .Build());
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void AddAllDifferentConstraintAction()
        {
            WorkArea.AddAllDifferentConstraint(new AllDifferentConstraintBuilder().WithName("New Constraint")
                                                                                  .Build());
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        private void AddDomainAction()
        {
            this.WorkArea.AddDomain(new DomainBuilder().WithName("New Domain")
                                                       .Build());
            this.titleBar.UpdateTitle();
        }
    }
}
