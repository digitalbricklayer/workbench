using System;
using System.Diagnostics.Contracts;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Code contract for the <see cref="Workbench.ViewModels.IShell"/> interface.
    /// </summary>
    [ContractClassFor(typeof(IShell))]
    internal abstract class IShellContract : IShell
    {
        private WorkAreaViewModel workArea;

        public WorkAreaViewModel WorkArea
        {
            get
            {
                Contract.Ensures(Contract.Result<WorkAreaViewModel>() != null);
                return this.workArea;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this.workArea = value;
            }
        }
    }
}
