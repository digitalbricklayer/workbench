using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the table details panel.
    /// </summary>
    public sealed class TableDetailsViewModel : Screen
    {
        private string _column;
        private string _row;
        private string _backgroundColorExpression;
        private string _textExpression;
        private readonly TableCellModel _selectedCell;
        private readonly IWindowManager _windowManager;

        /// <summary>
        /// Initialize a table details view model with the selected cell.
        /// </summary>
        public TableDetailsViewModel(TableCellModel theSelectedCell, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theSelectedCell != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            _selectedCell = theSelectedCell;
            _windowManager = theWindowManager;
            DisplayName = "Details";
            Column = "Name";
            Row = "1";
            BackgroundColorExpression = string.Empty;
            TextExpression = string.Empty;
        }

        /// <summary>
        /// Initialize a table details view model with a window manager.
        /// </summary>
        /// <param name="theWindowManager">The window manager.</param>
        public TableDetailsViewModel(IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theWindowManager != null);
            _windowManager = theWindowManager;
            DisplayName = string.Empty;
            Column = string.Empty;
            Row = string.Empty;
            BackgroundColorExpression = string.Empty;
            TextExpression = string.Empty;
        }

        public string Column
        {
            get => _column;
            set
            {
                _column = value;
                NotifyOfPropertyChange();
            }
        }

        public string Row
        {
            get => _row;
            set
            {
                _row = value;
                NotifyOfPropertyChange();
            }
        }

        public string BackgroundColorExpression
        {
            get => _backgroundColorExpression;
            set
            {
                _backgroundColorExpression = value;
                NotifyOfPropertyChange();
            }
        }

        public string TextExpression
        {
            get => _textExpression;
            set
            {
                _textExpression = value;
                NotifyOfPropertyChange();
            }
        }

        public void EditText()
        {
            var expressionEditor = new PropertyExpressionEditorViewModel();
            expressionEditor.Expression = TextExpression;
            var status = _windowManager.ShowDialog(expressionEditor);
            if (status.HasValue && !status.Value) return;
            TextExpression = expressionEditor.Expression;
        }

        public void EditBackground()
        {
            var expressionEditor = new PropertyExpressionEditorViewModel();
            expressionEditor.Expression = BackgroundColorExpression;
            var status = _windowManager.ShowDialog(expressionEditor);
            if (status.HasValue && !status.Value) return;
            BackgroundColorExpression = expressionEditor.Expression;
        }
    }
}
