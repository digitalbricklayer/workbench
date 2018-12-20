using System.Diagnostics.Contracts;

namespace Workbench
{
    [ContractClass(typeof(IMainWindowContract))]
    public interface IMainWindow
    {
        /// <summary>
        /// Gets or sets the shell.
        /// </summary>
        IShell Shell { get; set; }

        /// <summary>
        /// Gets or sets the title bar.
        /// </summary>
        ITitleBar TitleBar { get; set; }
    }
}