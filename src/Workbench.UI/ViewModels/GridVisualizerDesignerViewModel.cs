using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class GridVisualizerDesignerViewModel : EditorViewModel
    {
        private GridViewModel grid;

        public GridVisualizerDesignerViewModel(GridVisualizerModel theGridModel,
                                               IEventAggregator theEventAggregator,
                                               IDataService theDataService,
                                               IViewModelService theViewModelService)
            : base(theGridModel, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theGridModel != null);
            Contract.Requires<ArgumentNullException>(theEventAggregator != null);
            Contract.Requires<ArgumentNullException>(theDataService != null);
            Contract.Requires<ArgumentNullException>(theViewModelService != null);

            Model = theGridModel;
            Grid = new GridViewModel(theGridModel.Grid);
        }

        /// <summary>
        /// Gets or sets the map view model.
        /// </summary>
        public GridViewModel Grid
        {
            get { return this.grid; }
            set
            {
                this.grid = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddColumn(GridColumnModel newColumn)
        {
            Contract.Requires<ArgumentNullException>(newColumn != null);
            Grid.AddColumn(newColumn);
        }

        public void Resize(int columns, int rows)
        {
            Contract.Assume(Grid != null);
            Grid.Resize(columns, rows);
        }

        public void AddRow(GridRowModel newRow)
        {
            Contract.Requires<ArgumentNullException>(newRow != null);
            Contract.Assume(Grid != null);
            Grid.AddRow(newRow);
        }
    }
}
