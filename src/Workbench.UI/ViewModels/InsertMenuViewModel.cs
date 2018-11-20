using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Core;
using Workbench.Core.Models;
using Workbench.Messages;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class InsertMenuViewModel : MenuViewModel
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        public InsertMenuViewModel(IEventAggregator theEventAggregator,
                                   IDataService theDataService,
                                   IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);

            this.windowManager = theWindowManager;
            this.eventAggregator = theEventAggregator;

            AddSingletonVariableCommand = new CommandHandler(AddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(AddAggregateVariableAction);
            AddExpressionConstraintCommand = new CommandHandler(AddExpressionConstraintAction);
            AddAllDifferentConstraintCommand = new CommandHandler(AddAllDifferentConstraintAction);
            AddDomainCommand = new CommandHandler(AddDomainAction);
            AddTableCommand = new CommandHandler(AddTableAction);
            AddChessboardCommand = new CommandHandler(AddChessboardAction);
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
        /// Gets the Insert|Add Table command.
        /// </summary>
        public ICommand AddTableCommand { get; }

        /// <summary>
        /// Gets the Insert|Add Chessboard command.
        /// </summary>
        public ICommand AddChessboardCommand { get; }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void AddSingletonVariableAction()
        {
            var singletonVariableEditorViewModel = new SingletonVariableEditorViewModel();
            var dialogResult = this.windowManager.ShowDialog(singletonVariableEditorViewModel);
            if (!dialogResult.GetValueOrDefault()) return;
            var singletonVariableModel = new SingletonVariableBuilder().WithName(singletonVariableEditorViewModel.VariableName)
                                                                       .WithDomain(singletonVariableEditorViewModel.DomainExpression)
                                                                       .Inside(Workspace.WorkspaceModel.Model)
                                                                       .Build();
            this.Workspace.AddSingletonVariable(singletonVariableModel);
            this.eventAggregator.PublishOnUIThread(new SingletonVariableAddedMessage(singletonVariableModel));
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void AddAggregateVariableAction()
        {
            var aggregateVariableEditorViewModel = new AggregateVariableEditorViewModel();
            var dialogResult = this.windowManager.ShowDialog(aggregateVariableEditorViewModel);
            if (!dialogResult.GetValueOrDefault()) return;
            var newAggregate = new AggregateVariableBuilder().WithName(aggregateVariableEditorViewModel.VariableName)
                                                             .WithDomain(aggregateVariableEditorViewModel.DomainExpression)
                                                             .WithSize(aggregateVariableEditorViewModel.Size)
                                                             .Inside(Workspace.WorkspaceModel.Model)
                                                             .Build();
            this.Workspace.AddAggregateVariable(newAggregate);
            this.eventAggregator.PublishOnUIThread(new AggregateVariableAddedMessage(newAggregate));
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void AddExpressionConstraintAction()
        {
            var expressionConstraintEditorViewModel = new ExpressionConstraintEditorViewModel();
            var x = this.windowManager.ShowDialog(expressionConstraintEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            Workspace.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName(expressionConstraintEditorViewModel.ConstraintName)
                                                                               .Inside(Workspace.WorkspaceModel.Model)
                                                                               .WithExpression(expressionConstraintEditorViewModel.ConstraintExpression)
                                                                               .Build());
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void AddAllDifferentConstraintAction()
        {
            var allDifferentConstraintEditorViewModel = new AllDifferentConstraintEditorViewModel(Workspace.WorkspaceModel.Model);
            var result = this.windowManager.ShowDialog(allDifferentConstraintEditorViewModel);
            if (!result.GetValueOrDefault()) return;
            Workspace.AddAllDifferentConstraint(new AllDifferentConstraintBuilder().WithName(allDifferentConstraintEditorViewModel.ConstraintName)
                                                                                   .WithExpression(allDifferentConstraintEditorViewModel.SelectedVariable)
                                                                                   .Inside(Workspace.WorkspaceModel.Model)
                                                                                   .Build());
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        private void AddDomainAction()
        {
            var domainEditorViewModel = new SharedDomainEditorViewModel();
            var x = this.windowManager.ShowDialog(domainEditorViewModel);
            if (!x.GetValueOrDefault()) return;
            this.Workspace.AddDomain(new SharedDomainBuilder().WithName(domainEditorViewModel.DomainName)
                                                              .Inside(Workspace.WorkspaceModel.Model)
                                                              .WithDomain(domainEditorViewModel.DomainExpression)
                                                              .Build());
        }

        private void AddTableAction()
        {
            Workspace.AddTableTab(TableModel.Default);
        }

        private void AddChessboardAction()
        {
            Workspace.AddChessboardTab(new ChessboardModel(new ModelName("board1")));
        }
    }
}
