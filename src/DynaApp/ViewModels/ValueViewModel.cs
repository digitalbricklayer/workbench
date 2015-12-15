using System;
using Caliburn.Micro;
using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a value.
    /// </summary>
    public sealed class ValueViewModel : PropertyChangedBase
    {
        private int value;
        private VariableViewModel variable;

        /// <summary>
        /// Initialize the value with a variable.
        /// </summary>
        /// <param name="theVariable">The variable the value is bound to.</param>
        public ValueViewModel(VariableViewModel theVariable)
        {
            if (theVariable == null)
                throw new ArgumentNullException("theVariable");
            this.Variable = theVariable;
        }

        /// <summary>
        /// Gets or sets the variable.
        /// </summary>
        public VariableViewModel Variable
        {
            get { return this.variable; }
            set
            {
                this.variable = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.UpdateModelValue(value);
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the value model.
        /// </summary>
        public ValueModel Model { get; set; }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get
            {
                return this.Variable.Name;
            }
        }

        /// <summary>
        /// Update the model value.
        /// </summary>
        /// <param name="newValue">New variable value.</param>
        private void UpdateModelValue(int newValue)
        {
            if (this.Model == null) return;
            this.Model.Value = newValue;
        }
    }
}
