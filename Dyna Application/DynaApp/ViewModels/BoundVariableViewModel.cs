using System;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a bound variable.
    /// </summary>
    public sealed class BoundVariableViewModel : AbstractViewModel
    {
        private int value;
        private VariableViewModel variable;
        private string name;

        /// <summary>
        /// Initialize the bound variable with a variable.
        /// </summary>
        /// <param name="theVariable">Variable to bind the value to.</param>
        public BoundVariableViewModel(VariableViewModel theVariable)
        {
            if (theVariable == null)
                throw new ArgumentNullException("theVariable");
            this.Variable = theVariable;
        }

        /// <summary>
        /// Gets or sets the variable name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
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
                OnPropertyChanged("Variable");
            }
        }

        /// <summary>
        /// Gets or sets the bound value.
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
                OnPropertyChanged("Value");
            }
        }
    }
}
