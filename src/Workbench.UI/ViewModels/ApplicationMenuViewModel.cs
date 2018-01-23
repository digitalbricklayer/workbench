using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Application menu view model.
    /// </summary>
    public sealed class ApplicationMenuViewModel : Screen
    {
        public ApplicationMenuViewModel(FileMenuViewModel theFileMenu,
                                        ModelMenuViewModel theModelMenu,
                                        SolutionMenuViewModel theSolutionMenu,
                                        EditMenuViewModel theEditMenu)
        {
            Contract.Requires<ArgumentNullException>(theFileMenu != null);
            Contract.Requires<ArgumentNullException>(theModelMenu != null);
            Contract.Requires<ArgumentNullException>(theSolutionMenu != null);
            Contract.Requires<ArgumentNullException>(theEditMenu != null);

            FileMenu = theFileMenu;
            ModelMenu = theModelMenu;
            SolutionMenu = theSolutionMenu;
            EditMenu = theEditMenu;
        }

        /// <summary>
        /// Gets the File menu.
        /// </summary>
        public FileMenuViewModel FileMenu { get; private set; }

        /// <summary>
        /// Gets the File menu.
        /// </summary>
        public EditMenuViewModel EditMenu { get; private set; }

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
