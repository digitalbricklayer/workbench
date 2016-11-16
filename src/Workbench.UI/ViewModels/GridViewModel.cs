using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class GridViewModel : Screen
    {
        private readonly GridModel model;

        public GridViewModel(GridModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);
            this.model = theModel;
        }

        /// <summary>
        /// Gets the map model.
        /// </summary>
        public GridModel Grid
        {
            get { return this.model; }
        }
    }
}
