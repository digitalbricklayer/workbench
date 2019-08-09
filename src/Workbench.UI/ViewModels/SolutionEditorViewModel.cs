using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Visualizer binding expression editor.
    /// </summary>
    public class SolutionEditorViewModel : Conductor<VisualizerExpressionItemViewModel>.Collection.OneActive
    {
        private readonly IWindowManager _windowManager;
        private ICommand _add;
        private ICommand _delete;
        private ICommand _edit;
        private readonly IList<VisualizerExpressionItemViewModel> _added;
        private readonly IList<int> _deleted;
        private readonly IList<VisualizerExpressionItemViewModel> _updated;

        /// <summary>
        /// Initialize a solution editor with existing visualizer binding expressions.
        /// </summary>
        /// <param name="visualizerExpressionItems">Visualizer expression items.</param>
        /// <param name="theWindowManager">The window manager.</param>
        public SolutionEditorViewModel(IEnumerable<VisualizerExpressionItemViewModel> visualizerExpressionItems, IWindowManager theWindowManager)
        {
            _windowManager = theWindowManager;
            Add = new CommandHandler(AddExpression);
            Edit = new CommandHandler(EditExpression, CanEditExpression);
            Delete = new CommandHandler(DeleteExpression, CanDeleteExpression);
            _added = new List<VisualizerExpressionItemViewModel>();
            _deleted = new List<int>();
            _updated = new List<VisualizerExpressionItemViewModel>();
            Items.AddRange(visualizerExpressionItems);
        }

        /// <summary>
        /// Gets or sets the add command.
        /// </summary>
        public ICommand Add
        {
            get => _add;
            set
            {
                _add = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the delete command.
        /// </summary>
        public ICommand Delete
        {
            get => _delete;
            set
            {
                _delete = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the edit command.
        /// </summary>
        public ICommand Edit
        {
            get => _edit;
            set
            {
                _edit = value;
                NotifyOfPropertyChange();
            }
        }

        public IReadOnlyCollection<VisualizerExpressionItemViewModel> Added => new ReadOnlyCollection<VisualizerExpressionItemViewModel>(_added);

        public IReadOnlyCollection<int> Deleted => new ReadOnlyCollection<int>(_deleted);

        public IReadOnlyCollection<VisualizerExpressionItemViewModel> Updated => new ReadOnlyCollection<VisualizerExpressionItemViewModel>(_updated);

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void Accept()
        {
            TryClose(true);
        }

        /// <summary>
        /// Add a new visualizer expression.
        /// </summary>
        private void AddExpression()
        {
            var expressionEditor = new VisualizerExpressionEditorViewModel();
            var status = _windowManager.ShowDialog(expressionEditor);
            if (!status.GetValueOrDefault()) return;
            var newExpressionItem = new VisualizerExpressionItemViewModel(expressionEditor.Expression);
            ActivateItem(newExpressionItem);
            _added.Add(newExpressionItem);
        }

        /// <summary>
        /// Delete a visualizer expression.
        /// </summary>
        private void DeleteExpression()
        {
            Debug.Assert(ActiveItem != null);
            if (ActiveItem.Id != default(int))
            {
                /*
                 * Only add existing expressions to the delete list. If the item
                 * was added during this dialog session, then the outside world
                 * doesn't need to know about it.
                 */
                _deleted.Add(ActiveItem.Id);
            }
            DeactivateItem(ActiveItem, close: true);
        }

        /// <summary>
        /// Can an expression be deleted.
        /// </summary>
        /// <returns>True if the expression can be deleted, false if the expression cannot be deleted.</returns>
        private bool CanDeleteExpression(object obj)
        {
            return ActiveItem != null;
        }

        private void EditExpression()
        {
            var originalText = ActiveItem.Text;
            var expressionEditor = new VisualizerExpressionEditorViewModel();
            expressionEditor.Expression = ActiveItem.Text;
            var status = _windowManager.ShowDialog(expressionEditor);
            if (!status.GetValueOrDefault()) return;
            ActiveItem.Text = expressionEditor.Expression;
            if (originalText != ActiveItem.Text && ActiveItem.Id != default(int))
                _updated.Add(ActiveItem);
        }

        /// <summary>
        /// Can an expression be edited.
        /// </summary>
        /// <returns>True if the expression can be edited, false if the expression cannot be edited.</returns>
        private bool CanEditExpression(object obj)
        {
            return ActiveItem != null;
        }
    }
}
