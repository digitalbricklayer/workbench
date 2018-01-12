using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
    [ContractClass(typeof(IShellContract))]
    public interface IShell
    {
        /// <summary>
        /// Gets or sets the work area.
        /// </summary>
        WorkAreaViewModel WorkArea { get; set; }
    }
}
