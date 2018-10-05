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

        /// <summary>
        /// Initialize a table details view model with the selected cell.
        /// </summary>
        public TableDetailsViewModel(TableCellModel theSelectedCell)
        {
            Contract.Requires<ArgumentNullException>(theSelectedCell != null);

            _selectedCell = theSelectedCell;
            DisplayName = "Details";
            Column = "Name";
            Row = "1";
            BackgroundColorExpression = string.Empty;
            TextExpression = string.Empty;
        }

        /// <summary>
        /// Initialize a table details view model without a selected cell or range of cells.
        /// </summary>
        public TableDetailsViewModel()
        {
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
    }
}
