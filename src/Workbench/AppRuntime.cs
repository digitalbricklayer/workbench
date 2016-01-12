using Workbench.ViewModels;

namespace Workbench
{
    /// <summary>
    /// Application runtime.
    /// </summary>
    public class AppRuntime : IAppRuntime
    {
        private const string ProgramName = "Constraint Capers Workbench";

        /// <summary>
        /// Gets or sets the workspace view model.
        /// </summary>
        public WorkspaceViewModel Workspace { get; set; }

        /// <summary>
        /// Gets the shell view model.
        /// </summary>
        public ShellViewModel Shell { get; set; }

        /// <summary>
        /// Gets or sets the current file name.
        /// </summary>
        public string CurrentFileName { get; set; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return ProgramName;
            }
        }
    }
}
