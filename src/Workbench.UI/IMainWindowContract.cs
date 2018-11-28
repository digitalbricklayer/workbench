using System;
using System.Diagnostics.Contracts;
using Workbench.ViewModels;

namespace Workbench
{
    [ContractClassFor(typeof(IMainWindow))]
    internal abstract class IMainWindowContract : IMainWindow
    {
        private IShell _shell;
        private ITitleBar _titleBar;

        public IShell Shell
        {
            get
            {
                Contract.Ensures(Contract.Result<IShell>() != null);
                return _shell;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _shell = value;
            }
        }

        public ITitleBar TitleBar
        {
            get
            {
                Contract.Ensures(Contract.Result<ITitleBar>() != null);
                return _titleBar;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _titleBar = value;
            }
        }
    }
}
