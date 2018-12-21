using System;
using System.Diagnostics.Contracts;
using System.IO;
using Caliburn.Micro;
using Workbench.Messages;
using Workbench.Properties;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Title bar in the main window.
    /// </summary>
    public sealed class TitleBarViewModel : Screen, IHandle<WorkspaceChangedMessage>, IHandle<DocumentChangedMessage>, ITitleBar
    {
        private string _title;
        private readonly IEventAggregator _eventAggregator;
        private readonly IShell _shell;

        /// <summary>
        /// Initialize a title bar view model with a shell and event aggregator.
        /// </summary>
        public TitleBarViewModel(IShell theShell, IEventAggregator theEventAggregator)
        {
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theShell != null);
            _eventAggregator = theEventAggregator;
            _shell = theShell;
            Title = Resources.MainWindowDefaultTitle;
        }

        /// <summary>
        /// Gets or sets the main window title.
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
        /// Handle workspace change messages.
        /// </summary>
        /// <param name="message">Change message.</param>
        public void Handle(WorkspaceChangedMessage message)
        {
            UpdateTitle();
        }

        /// <summary>
        /// Handle document change messages
        /// </summary>
        /// <param name="message"></param>
        public void Handle(DocumentChangedMessage message)
        {
            UpdateTitle();
        }

        /// <summary>
        /// Update main window title.
        /// </summary>
        private void UpdateTitle()
        {
            var newTitle = Resources.ApplicationName + " " + Resources.TitleNameFileSeperator + " ";

            if (_shell.CurrentDocument.IsNew || _shell.CurrentDocument.Path.IsEmpty)
            {
                newTitle += Resources.TitleUntitledFileTitle;
                Title = newTitle;
                return;
            }

            if (!_shell.CurrentDocument.Path.IsEmpty)
            {
                newTitle += Path.GetFileName(_shell.CurrentDocument.Path.FullPath);
            }

            if (_shell.CurrentDocument.IsDirty)
            {
                newTitle += " " + Resources.TitleDirtyFileDesignator;
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