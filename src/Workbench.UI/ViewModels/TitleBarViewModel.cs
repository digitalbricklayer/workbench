using System;
using System.Diagnostics.Contracts;
using System.IO;
using Caliburn.Micro;
using Workbench.Messages;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Title bar in the main window.
    /// </summary>
    public class TitleBarViewModel : Screen, IHandle<WorkspaceChangedMessage>
    {
        private string _title;
        private readonly IEventAggregator _eventAggregator;
        private readonly IShell _shell;

        /// <summary>
        /// Initialize a title bar view model with default values.
        /// </summary>
        public TitleBarViewModel(IShell theShell, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theShell != null);
            _eventAggregator = theEventAggregator;
            _shell = theShell;
            Title = "Constraint Workbench" + " - " + "Untitled";
        }

        /// <summary>
        /// Gets or sets the main window title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Handle workspace change messages.
        /// </summary>
        /// <param name="message">Change message.</param>
        public void Handle(WorkspaceChangedMessage message)
        {
            UpdateTitle();
        }

        /// <summary>
        /// Update main window title.
        /// </summary>
        private void UpdateTitle()
        {
            var newTitle = "Constraint Workbench" + " - ";

            if (_shell.CurrentDocument.IsNew || _shell.CurrentDocument.Path.IsEmpty)
            {
                newTitle += "Untitled";
                Title = newTitle;
                return;
            }

            if (!_shell.CurrentDocument.Path.IsEmpty)
            {
                newTitle += Path.GetFileName(_shell.CurrentDocument.Path.FullPath);
                Title = newTitle;
                return;
            }

            if (_shell.CurrentDocument.IsDirty)
            {
                newTitle += " *";
            }

            Title = newTitle;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _eventAggregator.Subscribe(this);
        }
    }
}