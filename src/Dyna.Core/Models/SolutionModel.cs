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
        /// Initialize the solution with the model and the bound domains.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theBoundVariables">Bound domains.</param>
        public SolutionModel(ModelModel theModel, params ValueModel[] theBoundVariables)
            : this(theModel)
        {
            if (theBoundVariables == null)
                throw new ArgumentNullException("theBoundVariables");
            this.Values.AddRange(theBoundVariables);
        }

        /// <summary>
        /// Initialize the solution with the model and the bound domains.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theBoundVariables">Bound domains.</param>
        public SolutionModel(ModelModel theModel, IEnumerable<ValueModel> theBoundVariables)
            : this(theModel)
        {
            if (theBoundVariables == null)
                throw new ArgumentNullException("theBoundVariables");
            this.Values.AddRange(theBoundVariables);
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

        public SolutionModel()
        {
            this.Values = new List<ValueModel>();
        }

        /// <summary>
        /// Gets the values in the solution.
        /// </summary>
        public List<ValueModel> Values { get; set; }

        /// <summary>
        /// Gets the model this solution solves.
        /// </summary>
        public ModelModel Model { get; set; }

        /// <summary>
        /// Add a value to the solution.
        /// </summary>
        /// <param name="theValue">New value.</param>
        public void AddValue(ValueModel theValue)
        {
            this.Values.Add(theValue);
        }

        /// <summary>
        /// Get the bound variable matching the name.
        /// </summary>
        /// <param name="theVariableName">Name of the variable to find.</param>
        /// <returns>Bound variable matching the name. Null if no domains matches the name.</returns>
        public ValueModel GetVariableByName(string theVariableName)
        {
            return this.Values.FirstOrDefault(x => x.Variable.Name == theVariableName);
        }
    }
}