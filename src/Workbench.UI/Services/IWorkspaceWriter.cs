using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Services
{
    [ContractClass(typeof(IWorkspaceWriterContract))]
    public interface IWorkspaceWriter
    {
        /// <summary>
        /// Write a workspace model to a file.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <param name="theWorkspace">Workspace model.</param>
        void Write(string filename, WorkspaceModel theWorkspace);
    }

    /// <summary>
    /// Code contract for the IWorkspaceWriter interface.
    /// </summary>
    [ContractClassFor(typeof(IWorkspaceWriter))]
    internal abstract class IWorkspaceWriterContract : IWorkspaceWriter
    {
        public void Write(string filename, WorkspaceModel theWorkspace)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(filename));
            Contract.Requires<ArgumentNullException>(theWorkspace != null);
        }
    }
}