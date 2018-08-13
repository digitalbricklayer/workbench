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
                                        EditMenuViewModel theEditMenu,
                                        InsertMenuViewModel theInsertMenu,
                                        TableMenuViewModel theTableMenu)
        {
            Contract.Requires<ArgumentNullException>(theFileMenu != null);
            Contract.Requires<ArgumentNullException>(theModelMenu != null);
            Contract.Requires<ArgumentNullException>(theSolutionMenu != null);
            Contract.Requires<ArgumentNullException>(theEditMenu != null);
            Contract.Requires<ArgumentNullException>(theInsertMenu != null);

            FileMenu = theFileMenu;
            ModelMenu = theModelMenu;
            SolutionMenu = theSolutionMenu;
            EditMenu = theEditMenu;
            InsertMenu = theInsertMenu;
            TableMenu = theTableMenu;
        }

        /// <summary>
        /// Gets the Insert menu.
        /// </summary>
        public InsertMenuViewModel InsertMenu { get; }

        /// <summary>
        /// Gets the File menu.
        /// </summary>
        public FileMenuViewModel FileMenu { get; }

        /// <summary>
        /// Gets the File menu.
        /// </summary>
        public EditMenuViewModel EditMenu { get; }

        /// <summary>
        /// Gets the Model menu.
        /// </summary>
        public ModelMenuViewModel ModelMenu { get; }

        /// <summary>
        /// Gets the Solution menu.
        /// </summary>
        public SolutionMenuViewModel SolutionMenu { get; }

        /// <summary>
        /// Gets the Table menu.
        /// </summary>
        public TableMenuViewModel TableMenu { get; }
    }
}
