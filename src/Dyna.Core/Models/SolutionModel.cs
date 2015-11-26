using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyna.Core.Models
{
    /// <summary>
    /// A solution to a model.
    /// </summary>
    [Serializable]
    public class SolutionModel : ModelBase
    {
        /// <summary>
        /// Initialize the solution with the model and the values.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theValues">Values making up the model solution.</param>
        public SolutionModel(ModelModel theModel, params ValueModel[] theValues)
            : this(theModel)
        {
            if (theValues == null)
                throw new ArgumentNullException("theValues");
            this.SingletonValues.AddRange(theValues);
        }

        /// <summary>
        /// Initialize the solution with the model and the values.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theValues">Values making up the model solution.</param>
        public SolutionModel(ModelModel theModel, IEnumerable<ValueModel> theValues)
            : this(theModel)
        {
            if (theValues == null)
                throw new ArgumentNullException("theValues");
            this.SingletonValues.AddRange(theValues);
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
            this.SingletonValues = new List<ValueModel>();
            this.AggregateValues = new List<AggregateValueModel>();
        }

        /// <summary>
        /// Gets and sets the values in the solution.
        /// </summary>
        public List<ValueModel> SingletonValues { get; set; }

        /// <summary>
        /// Gets and sets the aggregate values in the solution.
        /// </summary>
        public List<AggregateValueModel> AggregateValues { get; set; }

        /// <summary>
        /// Gets the model this solution solves.
        /// </summary>
        public ModelModel Model { get; set; }

        /// <summary>
        /// Add a value to the solution.
        /// </summary>
        /// <param name="theValue">New value.</param>
        public void AddSingletonValue(ValueModel theValue)
        {
            this.SingletonValues.Add(theValue);
        }

        /// <summary>
        /// Add a value to the solution.
        /// </summary>
        /// <param name="theValue">New value.</param>
        public void AddAggregateValue(AggregateValueModel theValue)
        {
            this.AggregateValues.Add(theValue);
        }

        /// <summary>
        /// Get the value matching the name.
        /// </summary>
        /// <param name="theVariableName">Name of the variable to find.</param>
        /// <returns>Value matching the name. Null if no value matches the name.</returns>
        public ValueModel GetSingletonVariableByName(string theVariableName)
        {
            return this.SingletonValues.FirstOrDefault(x => x.Variable.Name == theVariableName);
        }

        /// <summary>
        /// Get the aggregate value matching the name.
        /// </summary>
        /// <param name="theVariableName">Aggregate value.</param>
        /// <returns>Aggregate value matching the name. Null if no aggregates matche the name.</returns>
        public AggregateValueModel GetAggregateVariableByName(string theVariableName)
        {
            return this.AggregateValues.FirstOrDefault(x => x.Variable.Name == theVariableName);
        }
    }
}