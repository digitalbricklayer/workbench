using System.Collections.Generic;
using System.Linq;
using DynaApp.Entities;

namespace DynaApp.Solver
{
    /// <summary>
    /// A solution to a model.
    /// </summary>
    class Solution
    {
        private readonly List<BoundVariable> boundVariables = new List<BoundVariable>();

        /// <summary>
        /// Initialize the solution with the model and the bound variables.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theBoundVariables">Bound variables.</param>
        public Solution(Model theModel, params BoundVariable[] theBoundVariables)
            : this(theModel)
        {
            this.boundVariables.AddRange(theBoundVariables);
        }

        /// <summary>
        /// Initialize the solution with the model and the bound variables.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theBoundVariables">Bound variables.</param>
        public Solution(Model theModel, IEnumerable<BoundVariable> theBoundVariables)
            : this(theModel)
        {
            this.boundVariables.AddRange(theBoundVariables);
        }

        /// <summary>
        /// Initialize the solution with the model.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        public Solution(Model theModel)
        {
            this.Model = theModel;
        }

        /// <summary>
        /// Gets the model this solutin solves.
        /// </summary>
        public Model Model { get; private set; }

        /// <summary>
        /// Gets the bound variables in the solution.
        /// </summary>
        public IEnumerable<BoundVariable> BoundVariables
        {
            get
            {
                return this.boundVariables;
            }
        }

        /// <summary>
        /// Get the bound variable matching the name.
        /// </summary>
        /// <param name="theVariableName">Name of the variable to find.</param>
        /// <returns>Bound variable matching the name. Null if no variables matches the name.</returns>
        public BoundVariable GetVariableByName(string theVariableName)
        {
            return this.boundVariables.FirstOrDefault(x => x.ModelVariable.Name == theVariableName);
        }
    }
}
