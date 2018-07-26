using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ModelItemViewModel : Screen
    {
        private Model _model;

        protected ModelItemViewModel(Model theModel)
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

        public abstract void Edit();
    }
}
