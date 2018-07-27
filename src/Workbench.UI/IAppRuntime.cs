using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench
{
    [ContractClass(typeof(IAppRuntimeContract))]
    public interface IAppRuntime
    {
        /// <summary>
        /// Gets the work area view model.
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

    /// <summary>
    /// Code contract for the IDataService interface.
    /// </summary>
    [ContractClassFor(typeof(IAppRuntime))]
    internal abstract class IAppRuntimeContract : IAppRuntime
    {
        private readonly string _applicationName = String.Empty;
        private string _currentFileName;
        private ShellViewModel _shell;
        private WorkspaceViewModel _workspace;

        public WorkspaceViewModel Workspace
        {
            get
            {
                Contract.Ensures(Contract.Result<WorkspaceViewModel>() != null);
                return _workspace;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _workspace = value;
            }
        }

        public ShellViewModel Shell
        {
            get
            {
                Contract.Ensures(Contract.Result<ShellViewModel>() != null);
                return _shell;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _shell = value;
            }
        }

        public string CurrentFileName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _currentFileName;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _currentFileName = value;
            }
        }

        public string ApplicationName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _applicationName;
            }
        }
    }
}
