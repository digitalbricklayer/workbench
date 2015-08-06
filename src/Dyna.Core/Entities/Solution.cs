using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyna.Core.Entities
{
    /// <summary>
    /// A solution to a model.
    /// </summary>
    [Serializable]
    public class Solution
    {
        private readonly List<BoundVariable> boundVariables = new List<BoundVariable>();

        /// <summary>
        /// Initialize the solution with the model and the bound domains.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theBoundVariables">Bound domains.</param>
        public Solution(Model theModel, params BoundVariable[] theBoundVariables)
            : this(theModel)
        {
            if (theBoundVariables == null)
                throw new ArgumentNullException("theBoundVariables");
            this.boundVariables.AddRange(theBoundVariables);
        }

        /// <summary>
        /// Initialize the solution with the model and the bound domains.
        /// </summary>
        /// <param name="theModel">Model that the solution is supposed to solve.</param>
        /// <param name="theBoundVariables">Bound domains.</param>
        public Solution(Model theModel, IEnumerable<BoundVariable> theBoundVariables)
            : this(theModel)
        {
            if (theBoundVariables == null)
                throw new ArgumentNullException("theBoundVariables");
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
        /// Gets the bound domains in the solution.
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
        /// <returns>Bound variable matching the name. Null if no domains matches the name.</returns>
        public BoundVariable GetVariableByName(string theVariableName)
        {
            return this.boundVariables.FirstOrDefault(x => x.Variable.Name == theVariableName);
        }
    }
}
