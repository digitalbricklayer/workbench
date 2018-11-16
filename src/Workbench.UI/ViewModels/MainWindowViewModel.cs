using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public class MainWindowViewModel : Conductor<object>.Collection.AllActive, IMainWindow
    {
        private IShell _shell;
        private TitleBarViewModel _titleBar;

        /// <summary>
        /// Initialize the main window view model with a shell and title bar.
        /// </summary>
        /// <param name="theShell">The shell implementing the application.</param>
        /// <param name="theTitleBarViewModel">Title bar view model.</param>
        public MainWindowViewModel(IShell theShell,
                                   TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theShell != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            Shell = theShell;
            TitleBar = theTitleBarViewModel;
            var subScreens = new List<object> { Shell, TitleBar };
            Items.AddRange(subScreens);
        }

        /// <summary>
        /// Gets or sets the shell.
        /// </summary>
        public IShell Shell
        {
            get => _shell;
            set
            {
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
    }
}
