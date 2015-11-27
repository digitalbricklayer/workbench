using System;
using System.Collections.ObjectModel;
using System.Windows;
using Dyna.Core.Models;

namespace DynaApp.ViewModels
{
    public sealed class AggregateVariableViewModel : VariableViewModel
    {
        /// <summary>
        /// Initialize a new aggregate variable with a name and location.
        /// </summary>
        /// <param name="newVariableName">Variable name.</param>
        /// <param name="size">Size of the aggregate.</param>
        /// <param name="domainExpression">Domain expression.</param>
        public AggregateVariableViewModel(string newVariableName, int size, VariableDomainExpressionViewModel domainExpression)
            : base(newVariableName)
        {
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Model = new AggregateVariableModel(newVariableName, size, this.DomainExpression.Model);
            this.Model.DomainExpression = this.DomainExpression.Model;
            this.DomainExpression = domainExpression;
        }

        /// <summary>
        /// Initialize a new aggregate variable with a name and location.
        /// </summary>
        /// <param name="newVariableName">Variable name.</param>
        /// <param name="newVariableLocation">Location.</param>
        public AggregateVariableViewModel(string newVariableName, Point newVariableLocation)
            : base(newVariableName, newVariableLocation)
        {
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Model = new AggregateVariableModel(newVariableName, newVariableLocation);
            this.Model.DomainExpression = this.DomainExpression.Model;
        }

        /// <summary>
        /// Initialize a new aggregate variable with default values.
        /// </summary>
        public AggregateVariableViewModel()
        {
            this.Name = "New variable";
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Model = new AggregateVariableModel(this.Name, 1, this.DomainExpression.Model);
            this.Model.DomainExpression = this.DomainExpression.Model;
        }

        /// <summary>
        /// Gets the aggregate variable name.
        /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                this.Model.Name = value;
                for (var i = 1; i <= this.Variables.Count; i++)
                    this.Variables[i-1].Name = this.Name + i;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the aggregate variable model.
        /// </summary>
        public new AggregateVariableModel Model { get; private set; }

        /// <summary>
        /// Gets the variables inside the aggregate.
        /// </summary>
        public ObservableCollection<VariableViewModel> Variables { get; private set; }

        /// <summary>
        /// Gets or sets the number of variables in the aggregate variable.
        /// </summary>
        public int VariableCount { get; set; }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public new VariableDomainExpressionViewModel DomainExpression
        {
            get
            {
                return base.DomainExpression;
            }
            set
            {
                base.DomainExpression = value;
                this.Model.DomainExpression = value.Model;
                foreach (var variable in this.Variables)
                {
                    variable.DomainExpression = value;
                    variable.Model.DomainExpression = value.Model;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets whether the variable is an aggregate.
        /// </summary>
        public override bool IsAggregate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the number of variables.
        /// </summary>
        public string NumberVariables
        {
            get { return Convert.ToString(this.Variables.Count); }
            set
            {
                var newSize = Convert.ToInt32(value);
                if (newSize == this.Variables.Count) return;
                this.Resize(newSize);
                OnPropertyChanged();
            }
        }

        private void Resize(int newSize)
        {
            this.Model.Resize(newSize);
            if (newSize > this.Variables.Count)
                this.GrowBy(newSize - this.Variables.Count);
            else
                this.ShrinkBy(this.Variables.Count - newSize);
        }

        private void GrowBy(int variablesToIncreaseBy)
        {
            for (var i = 0; i < variablesToIncreaseBy; i++)
            {
                var newVariable = new VariableViewModel();
                newVariable.DomainExpression = this.DomainExpression;
                this.Variables.Add(newVariable);
            }
        }

        private void ShrinkBy(int variablesToShrinkBy)
        {
            for (var i = 0; i < variablesToShrinkBy; i++)
                this.Variables.RemoveAt(this.Variables.Count-1);
        }
    }
}
