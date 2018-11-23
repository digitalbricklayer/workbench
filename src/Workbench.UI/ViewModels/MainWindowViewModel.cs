using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Media;
using Caliburn.Micro;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainWindowViewModel : Conductor<object>.Collection.AllActive, IMainWindow
    {
        private IShell _shell;
        private TitleBarViewModel _titleBar;
        private ImageSource _icon;
        private readonly IResourceManager _resourceManager;

        /// <summary>
        /// Initialize the main window view model with a shell, title bar and resource manager.
        /// </summary>
        /// <param name="theShell">The shell implementing the application.</param>
        /// <param name="theTitleBarViewModel">Title bar view model.</param>
        /// <param name="theResourceManager">Resource manager.</param>
        public MainWindowViewModel(IShell theShell, TitleBarViewModel theTitleBarViewModel, IResourceManager theResourceManager)
        {
            Contract.Requires<ArgumentNullException>(theShell != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);
            Contract.Requires<ArgumentNullException>(theResourceManager != null);

            Shell = theShell;
            TitleBar = theTitleBarViewModel;
            _resourceManager = theResourceManager;
            var subScreens = new List<object> { Shell, TitleBar };
            Items.AddRange(subScreens);
        }

		/// <summary>
        /// Gets or sets the application icon.
        /// </summary>
        public ImageSource Icon
        {
            get => _icon;
		    set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _icon = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the shell.
        /// </summary>
        public IShell Shell
        {
            get => _shell;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _shell = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the title bar view model.
        /// </summary>
        public TitleBarViewModel TitleBar
        {
            get => _titleBar;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _titleBar = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Main window is closing.
        /// </summary>
        /// <param name="cancelEventArgs">Cancel event arguments.</param>
        public void OnClose(CancelEventArgs cancelEventArgs)
        {
            Shell.Close(cancelEventArgs);
        }

        protected override void OnInitialize()
        {
            Icon = _resourceManager.GetBitmap("Images/AppIcon.ico");
            base.OnInitialize();
        }
    }
}
