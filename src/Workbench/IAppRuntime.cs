using Workbench.ViewModels;

namespace Workbench
{
    public interface IAppRuntime
    {
        /// <summary>
        /// Gets the workspace view model.
        /// </summary>
        WorkspaceViewModel Workspace { get; set; }

        /// <summary>
        /// Gets the shell view model.
        /// </summary>
        ShellViewModel Shell { get; set; }

        /// <summary>
        /// Gets or sets the current file name.
        /// </summary>
        string CurrentFileName { get; set; }

        /// <summary>
        /// Gets the program name.
        /// </summary>
        string ApplicationName { get; }
    }
}
