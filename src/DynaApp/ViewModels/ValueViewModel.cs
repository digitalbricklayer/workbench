using System;
using DynaApp.Models;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// View model for a value.
    /// </summary>
    public sealed class ValueViewModel : AbstractViewModel
    {
        private int value;
        private string variableName;

        /// <summary>
        /// Initialize the value with a variable.
        /// </summary>
        /// <param name="theVariableName">Name of the variable the value is bound to.</param>
        public ValueViewModel(string theVariableName)
        {
            if (string.IsNullOrWhiteSpace(theVariableName))
                throw new ArgumentNullException("theVariableName");
            this.VariableName = theVariableName;
        }

        /// <summary>
        /// Initialize the value with default values.
        /// </summary>
        public ValueViewModel()
        {
            this.VariableName = string.Empty;
        }

        /// <summary>
        /// Gets the variable name.
        /// </summary>
        public string VariableName
        {
            get { return this.variableName; }
            set
            {
                this.variableName = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the value model.
        /// </summary>
        public ValueModel Model { get; set; }
    }
}
