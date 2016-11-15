using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Services;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Application menu view model.
    /// </summary>
    public sealed class ApplicationMenuViewModel : Screen
    {
        public ApplicationMenuViewModel(FileMenuViewModel theFileMenu,
                                        ModelMenuViewModel theModelMenu,
                                        SolutionMenuViewModel theSolutionMenu)
        {
            Contract.Requires<ArgumentNullException>(theFileMenu != null);
            Contract.Requires<ArgumentNullException>(theModelMenu != null);
            Contract.Requires<ArgumentNullException>(theSolutionMenu != null);

            FileMenu = theFileMenu;
            ModelMenu = theModelMenu;
            SolutionMenu = theSolutionMenu;
        }

        /// <summary>
        /// Gets the File menu.
        /// </summary>
        public FileMenuViewModel FileMenu { get; private set; }

        /// <summary>
        /// Gets the Model menu.
        /// </summary>
        public ModelMenuViewModel ModelMenu { get; private set; }

        /// <summary>
        /// Gets the Solution menu.
        /// </summary>
        public SolutionMenuViewModel SolutionMenu { get; private set; }
    }
}
