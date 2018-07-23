using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ItemViewModel : Conductor<Screen>.Collection.AllActive
    {
        private Model _model;

        protected ItemViewModel(Model theModel)
        {
            Model = theModel;
        }

        public Model Model
        {
            get => _model;
            private set
            {
                _model = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
