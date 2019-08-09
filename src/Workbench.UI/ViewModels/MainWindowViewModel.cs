using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using Caliburn.Micro;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    public sealed class MainWindowViewModel : Conductor<object>.Collection.AllActive, IMainWindow
    {
        private IShell _shell;
        private ITitleBar _titleBar;
        private ImageSource _icon;
        private readonly IResourceManager _resourceManager;

        /// <summary>
        /// Initialize the main window view model with a shell, title bar and resource manager.
        /// </summary>
        /// <param name="theShell">The shell implementing the application.</param>
        /// <param name="theTitleBar">The title bar.</param>
        /// <param name="theResourceManager">Resource manager.</param>
        public MainWindowViewModel(IShell theShell, ITitleBar theTitleBar, IResourceManager theResourceManager)
        {
            Shell = theShell;
            TitleBar = theTitleBar;
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
                _shell = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the title bar.
        /// </summary>
        public ITitleBar TitleBar
        {
            get => _titleBar;
            set
            {
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
            Shell.OnClose(cancelEventArgs);
        }

        protected override void OnInitialize()
        {
            Icon = _resourceManager.GetBitmap("Images/AppIcon.ico");
            base.OnInitialize();
        }
    }
}
