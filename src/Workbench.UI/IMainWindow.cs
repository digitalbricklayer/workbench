using Workbench.ViewModels;

namespace Workbench
{
    public interface IMainWindow
    {
        /// <summary>
        /// Gets or sets the shell.
        /// </summary>
        IShell Shell { get; set; }

        /// <summary>
        /// Gets or sets the title bar view model.
        /// </summary>
        TitleBarViewModel TitleBar { get; set; }
    }
}