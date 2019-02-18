using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Core.Solvers;
using Workbench.ViewModels;

namespace Workbench
{
    /// <summary>
    /// Contract for the workspace view model.
    /// </summary>
    public interface IWorkspace : IScreen
    {
        /// <summary>
        /// Gets or sets the workspace model.
        /// </summary>
        WorkspaceModel WorkspaceModel { get; }

        /// <summary>
        /// Gets or sets the model editor.
        /// </summary>
        ModelEditorTabViewModel ModelEditor { get; set; }

        /// <summary>
        /// Gets or sets the solution viewer.
        /// </summary>
        SolutionViewerTabViewModel SolutionViewer { get; set; }

        /// <summary>
        /// Gets all chessboard tab visualizers.
        /// </summary>
        BindableCollection<ChessboardTabViewModel> ChessboardTabs { get; }

        /// <summary>
        /// Gets all table tab visualizers.
        /// </summary>
        BindableCollection<TableTabViewModel> TableTabs { get; }

        /// <summary>
        /// Gets or sets the solution model.
        /// </summary>
        SolutionModel Solution { get; }

        /// <summary>
        /// Gets the visualizer binding expressions.
        /// </summary>
        BindableCollection<VisualizerBindingExpressionViewModel> Bindings { get; }

        /// <summary>
        /// Solve the constraint model.
        /// </summary>
        SolveResult SolveModel();

        /// <summary>
        /// Create a new singleton variable.
        /// </summary>
        /// <param name="newVariable">New variable.</param>
        /// <returns>New singleton variable view model.</returns>
        void AddSingletonVariable(SingletonVariableModel newVariable);

        /// <summary>
        /// Create a new aggregate variable.
        /// </summary>
        /// <param name="newVariable">New variable name.</param>
        /// <returns>New aggregate variable view model.</returns>
        void AddAggregateVariable(AggregateVariableModel newVariable);

        /// <summary>
        /// Create a new domain at a specific location.
        /// </summary>
        /// <param name="newDomain">New domain.</param>
        void AddDomain(SharedDomainModel newDomain);

        /// <summary>
        /// Create a new expression constraint.
        /// </summary>
        /// <param name="newConstraint">New constraint name.</param>
        void AddExpressionConstraint(ExpressionConstraintModel newConstraint);

        /// <summary>
        /// Create a new all different constraint.
        /// </summary>
        /// <param name="newConstraint">New all different constraint.</param>
        void AddAllDifferentConstraint(AllDifferentConstraintModel newConstraint);

        /// <summary>
        /// Add a new chessboard tab to the workspace.
        /// </summary>
        /// <param name="newChessboard">New chessboard tab.</param>
        void AddChessboardTab(ChessboardModel newChessboard);

        /// <summary>
        /// Add a new table tab to the workspace.
        /// </summary>
        /// <param name="newTable">New table.</param>
        void AddTableTab(TableModel newTable);

        /// <summary>
        /// Close the tab as initiated by the user.
        /// </summary>
        void CloseTab(IWorkspaceTabViewModel tabToClose);

        /// <summary>
        /// Add a new visualizer binding expression.
        /// </summary>
        /// <param name="aNewExpression">A new visualizer binding expression.</param>
        void AddBindingExpression(VisualizerBindingExpressionModel aNewExpression);

        /// <summary>
        /// Get a visualizer binding expression using the identity.
        /// </summary>
        /// <param name="bindingExpressionId">Visualizer binding expression identity.</param>
        /// <returns>Visualizer binding expression view model matching the identity.</returns>
        VisualizerBindingExpressionViewModel GetBindingExpressionById(int bindingExpressionId);

        /// <summary>
        /// Delete a visualizer binding expression.
        /// </summary>
        /// <param name="aVisualizerBinding">Visualizer binding to delete.</param>
        void DeleteBindingExpression(VisualizerBindingExpressionViewModel aVisualizerBinding);
    }
}