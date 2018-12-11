using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Chessboard tab visualizer view model.
    /// </summary>
    public sealed class ChessboardTabViewModel : Screen, IWorkspaceTabViewModel
    {
        private readonly IWindowManager _windowManager;
        private ChessboardViewModel _board;
        private string _name;
        private string _title;
        private string _text;

        public ChessboardTabViewModel(ChessboardTabModel theChessboardTabModel, IWindowManager theWindowManager)
        {
            Contract.Requires<ArgumentNullException>(theChessboardTabModel != null);
            Contract.Requires<ArgumentNullException>(theWindowManager != null);

            _windowManager = theWindowManager;
            Title = TabText = Name = DisplayName = theChessboardTabModel.Name;
            Model = theChessboardTabModel;
            Board = new ChessboardViewModel(theChessboardTabModel.Model);
        }

        /// <summary>
        /// Gets whether the tab can be manually closed by the user.
        /// </summary>
        public bool CloseTabIsVisible => true;

        /// <summary>
        /// Gets the table tab model.
        /// </summary>
        public ChessboardTabModel Model { get; }

        /// <summary>
        /// Gets or sets the chessboard name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                TabText = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the tab text.
        /// </summary>
        public string TabText
        {
            get => _text;
            set
            {
                _text = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the table title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the chessboard view model.
        /// </summary>
        public ChessboardViewModel Board
        {
            get => _board;
            set
            {
                _board = value;
                NotifyOfPropertyChange();
            }
        }

        public void EditTitle()
        {
            var titleEditor = new TabTitleEditorViewModel();
            titleEditor.TabTitle = Title;
            var status = _windowManager.ShowDialog(titleEditor);
            if (status.HasValue && !status.Value) return;
            Title = titleEditor.TabTitle;
            Model.Title.Text = Title;
        }

        public void EditName()
        {
            var nameEditor = new TabNameEditorViewModel();
            nameEditor.TabName = Name;
            var status = _windowManager.ShowDialog(nameEditor);
            if (status.HasValue && !status.Value) return;
            Name = nameEditor.TabName;
            Model.Name = Name;
        }

        /// <summary>
        /// Update the view model from the chessboard model.
        /// </summary>
        public void UpdateFromModel()
        {
            Board.UpdateFromModel();
        }
    }
}
