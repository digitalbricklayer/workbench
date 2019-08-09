using System.Collections.Generic;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class SolutionViewerPanelViewModel : Screen
    {
        private IObservableCollection<SingletonVariableLabelModel> _singletonLabels;
        private IObservableCollection<AggregateVariableLabelModel> _compoundLabels;
        private IObservableCollection<LabelModel> _labels;

        public SolutionViewerPanelViewModel()
        {
            DisplayName = "Solution";
            _singletonLabels = new BindableCollection<SingletonVariableLabelModel>();
            _compoundLabels = new BindableCollection<AggregateVariableLabelModel>();
            _labels = new BindableCollection<LabelModel>();
        }

        /// <summary>
        /// Gets the labels in the solution.
        /// </summary>
        public IObservableCollection<SingletonVariableLabelModel> SingletonLabels
        {
            get { return _singletonLabels; }
            set
            {
                _singletonLabels = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets the compound labels in the solution.
        /// </summary>
        public IObservableCollection<AggregateVariableLabelModel> CompoundLabels
        {
            get { return _compoundLabels; }
            set
            {
                _compoundLabels = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets all of the labels in the solution.
        /// </summary>
        public IObservableCollection<LabelModel> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                NotifyOfPropertyChange();
            }
        }

        public void BindTo(SolutionModel theSolution)
        {
            var allLabels = new List<LabelModel>(theSolution.Snapshot.SingletonLabels);
            allLabels.AddRange(theSolution.Snapshot.AggregateLabels);
            Labels = new BindableCollection<LabelModel>(allLabels);
            SingletonLabels = new BindableCollection<SingletonVariableLabelModel>(theSolution.Snapshot.SingletonLabels);
            CompoundLabels = new BindableCollection<AggregateVariableLabelModel>(theSolution.Snapshot.AggregateLabels);
        }
    }
}
