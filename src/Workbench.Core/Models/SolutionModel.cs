using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.Core.Solver;

namespace Workbench.Core.Models
{
    /// <summary>
    /// A solution to a model.
    /// </summary>
    [Serializable]
    public class SolutionModel : AbstractModel
    {
        private DisplayModel display;
        private ObservableCollection<ValueModel> singletonValues;
        private ObservableCollection<ValueModel> aggregateValues;

        /// <summary>
        /// Initialize the solution with the model and the values.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theValues">SingletonValues making up the model solution.</param>
        public SolutionModel(ModelModel theModel, params ValueModel[] theValues)
            : this(theModel)
        {
            if (theValues == null)
                throw new ArgumentNullException("theValues");
            foreach (var valueModel in theValues)
                this.SingletonValues.Add(valueModel);
        }

        /// <summary>
        /// Initialize the solution with the model and the values.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theValues">SingletonValues making up the model solution.</param>
        public SolutionModel(ModelModel theModel, IEnumerable<ValueModel> theValues)
            : this(theModel)
        {
            if (theValues == null)
                throw new ArgumentNullException("theValues");
            foreach (var valueModel in theValues)
                this.SingletonValues.Add(valueModel);
        }

        /// <summary>
        /// Initialize the solution with the model.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        public SolutionModel(ModelModel theModel)
            : this()
        {
            this.Model = theModel;
        }

        /// <summary>
        /// Initialize the solution with default values.
        /// </summary>
        public SolutionModel()
        {
            this.Display = new DisplayModel();
            this.SingletonValues = new ObservableCollection<ValueModel>();
            this.AggregateValues = new ObservableCollection<ValueModel>();
        }

        /// <summary>
        /// Gets or sets the solution display.
        /// </summary>
        public DisplayModel Display
        {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the values in the solution.
        /// </summary>
        public ObservableCollection<ValueModel> SingletonValues
        {
            get { return singletonValues; }
            set
            {
                singletonValues = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets the aggregate values in the solution.
        /// </summary>
        public ObservableCollection<ValueModel> AggregateValues
        {
            get { return aggregateValues; }
            set
            {
                aggregateValues = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the model this solution solves.
        /// </summary>
        public ModelModel Model { get; private set; }

        /// <summary>
        /// Add a value to the solution.
        /// </summary>
        /// <param name="theValue">New value.</param>
        public void AddSingletonValue(ValueModel theValue)
        {
            Contract.Requires<ArgumentNullException>(theValue != null);
            this.SingletonValues.Add(theValue);
        }

        /// <summary>
        /// Add a value to the solution.
        /// </summary>
        /// <param name="theValue">New value.</param>
        public void AddAggregateValue(ValueModel theValue)
        {
            Contract.Requires<ArgumentNullException>(theValue != null);
            this.AggregateValues.Add(theValue);
        }

        /// <summary>
        /// Get the value matching the name.
        /// </summary>
        /// <param name="theVariableName">Name of the variable to find.</param>
        /// <returns>Value matching the name. Null if no value matches the name.</returns>
        public ValueModel GetSingletonVariableValueByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return this.SingletonValues.FirstOrDefault(x => x.Variable.Name == theVariableName);
        }

        /// <summary>
        /// Get the aggregate value matching the name.
        /// </summary>
        /// <param name="theVariableName">Aggregate value.</param>
        /// <returns>Aggregate value matching the name. Null if no aggregates matche the name.</returns>
        public ValueModel GetAggregateVariableValueByName(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return this.AggregateValues.FirstOrDefault(x => x.Variable.Name == theVariableName);
        }

        /// <summary>
        /// Add the visualizer.
        /// </summary>
        /// <param name="theVisualizer">The visualizer to add.</param>
        public void AddVisualizer(VisualizerModel theVisualizer)
        {
            Contract.Requires<ArgumentNullException>(theVisualizer != null);
            this.Display.AddVisualizer(theVisualizer);
        }

        /// <summary>
        /// Get the visualizer bound to the variable matching the variable name.
        /// </summary>
        /// <param name="theVariableName">Name of the variable.</param>
        /// <returns>Visualizer bound to the variable matching the variable name.</returns>
        public VisualizerModel GetVisualizerFor(string theVariableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theVariableName));
            return this.Display.GetVisualizerFor(theVariableName);
        }

        /// <summary>
        /// Update the solution from a snapshot.
        /// </summary>
        /// <param name="theSnapshot">The snapshot.</param>
        public void UpdateSolutionFrom(SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);
            this.Display.UpdateSolutionFrom(theSnapshot);

            foreach (var aSingletonValue in theSnapshot.SingletonValues)
            {
                this.AddSingletonValue(aSingletonValue);
            }

            foreach (var anAggregateValue in theSnapshot.AggregateValues)
            {
                this.AddAggregateValue(anAggregateValue);
            }
        }
    }
}
