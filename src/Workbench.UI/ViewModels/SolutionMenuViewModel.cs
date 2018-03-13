using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using Workbench.Commands;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    /// <summary>
    /// View model for the solution main menu.
    /// </summary>
    public class SolutionMenuViewModel
    {
        /// <summary>
        /// Initialize the solution menu view model with default values.
        /// </summary>
        public SolutionMenuViewModel()
        {
            EditSolutionCommand = IoC.Get<EditSolutionCommand>();
        }

        /// <summary>
        /// Gets the Solution|Edit Solution command.
        /// </summary>
        public ICommand EditSolutionCommand { get; private set; }
    }
}
