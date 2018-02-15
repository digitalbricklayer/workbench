using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class InsertMenuViewModel
    {
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;
        private readonly IEventAggregator eventAggregator;
        private readonly IViewModelService viewModelService;
        private readonly IDataService dataService;

        public InsertMenuViewModel(IAppRuntime theAppRuntime,
                                   TitleBarViewModel theTitleBarViewModel,
                                   IEventAggregator theEventAggregator,
                                   IViewModelService theViewModelService,
                                   IDataService theDataService)
        {
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;
            this.eventAggregator = theEventAggregator;
            this.viewModelService = theViewModelService;
            this.dataService = theDataService;

            AddSingletonVariableCommand = new CommandHandler(AddSingletonVariableAction);
            AddAggregateVariableCommand = new CommandHandler(AddAggregateVariableAction);
            AddExpressionConstraintCommand = new CommandHandler(AddExpressionConstraintAction);
            AddAllDifferentConstraintCommand = new CommandHandler(AddAllDifferentConstraintAction);
            AddDomainCommand = new CommandHandler(AddDomainAction);
            AddChessboardCommand = IoC.Get<AddChessboardVisualizerCommand>();
            AddTableCommand = IoC.Get<AddTableVisualizerCommand>();
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
        /// Gets the Insert|Table command
        /// </summary>
        public ICommand AddTableCommand { get; private set; }

        /// <summary>
        /// Gets the Insert|Chessboard command.
        /// </summary>
        public ICommand AddChessboardCommand { get; private set; }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void AddSingletonVariableAction()
        {
            var newVariableLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.WorkArea.AddSingletonVariable("New Variable", newVariableLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        private void AddAggregateVariableAction()
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
        private void AddExpressionConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            WorkArea.AddExpressionConstraint(new ExpressionConstraintBuilder().WithName("New Constraint")
                                                                              .Build(),
                                             newConstraintLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        private void AddAllDifferentConstraintAction()
        {
            var newConstraintLocation = Mouse.GetPosition(Application.Current.MainWindow);
            WorkArea.AddAllDifferentConstraint(new AllDifferentConstraintBuilder().WithName("New Constraint")
                                                                                  .Build(),
                                               newConstraintLocation);
            this.titleBar.UpdateTitle();
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        private void AddDomainAction()
        {
            var newDomainLocation = Mouse.GetPosition(Application.Current.MainWindow);
            this.WorkArea.AddDomain("New Domain", newDomainLocation);
            this.titleBar.UpdateTitle();
        }
    }
}
