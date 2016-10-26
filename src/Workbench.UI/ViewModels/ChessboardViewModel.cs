using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class ChessboardViewModel : Screen
    {
        private ChessboardModel model;

        public ChessboardViewModel(ChessboardModel theModel)
        {
            Contract.Requires<ArgumentNullException>(theModel != null);

            this.model = theModel;
        }

        public ChessboardModel Model
        {
            get { return this.model; }
            set { this.model = value; }
        }
    }
}
