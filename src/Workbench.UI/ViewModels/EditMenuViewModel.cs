using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public sealed class EditMenuViewModel
    {
        private readonly IDataService dataService;
        private readonly WorkAreaMapper workAreaMapper;
        private readonly IAppRuntime appRuntime;
        private readonly TitleBarViewModel titleBar;

        public EditMenuViewModel(IDataService theDataService,
                                 WorkAreaMapper theWorkAreaMapper,
                                 IAppRuntime theAppRuntime,
                                 TitleBarViewModel theTitleBarViewModel)
        {
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theWorkAreaMapper != null);
            Contract.Requires<ArgumentNullException>(theAppRuntime != null);
            Contract.Requires<ArgumentNullException>(theTitleBarViewModel != null);

            this.dataService = theDataService;
            this.workAreaMapper = theWorkAreaMapper;
            this.appRuntime = theAppRuntime;
            this.titleBar = theTitleBarViewModel;
            DeleteCommand = new CommandHandler(ModelDeleteAction, _ => CanDeleteExecute);
        }

        /// <summary>
        /// Gets the work area view model.
        /// </summary>
        public WorkAreaViewModel WorkArea
        {
            get { return this.appRuntime.WorkArea; }
        }

        /// <summary>
        /// Gets the Model|Delete command.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Gets whether the "Model|Delete" menu item can be executed.
        /// </summary>
        public bool CanDeleteExecute
        {
            get
            {
                if (WorkArea.SelectedDisplay == "Editor")
                {
                    return WorkArea.Editor.Items.Any(_ => _.IsSelected);
                }
                else
                {
                    throw new NotImplementedException("Selection is not implemented for the viewer");
                }
            }
        }

        /// <summary>
        /// Delete all selected graphics.
        /// </summary>
        private void ModelDeleteAction()
        {
            this.WorkArea.DeleteSelectedGraphics();
            this.titleBar.UpdateTitle();
        }
    }
}
