using System;
using System.Collections.ObjectModel;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a model.
    /// </summary>
    public sealed class ModelViewModel : AbstractModelBase
    {
        public ModelViewModel()
        {
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Connections = new ObservableCollection<ConnectionViewModel>();
        }

        /// <summary>
        /// The collection of variables in the network.
        /// </summary>
        public ObservableCollection<VariableViewModel> Variables { get; private set; }

        /// <summary>
        /// The collection of connections in the network.
        /// </summary>
        public ObservableCollection<ConnectionViewModel> Connections { get; private set; }

        /// <summary>
        /// Add a new variable to the model.
        /// </summary>
        /// <param name="newVariableViewModel">New variable.</param>
        public void AddVariable(VariableViewModel newVariableViewModel)
        {
            if (newVariableViewModel == null)
                throw new ArgumentNullException("newVariableViewModel");
            this.Variables.Add(newVariableViewModel);
        }
    }
}
