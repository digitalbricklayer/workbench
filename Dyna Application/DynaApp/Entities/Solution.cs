using System.Collections.Generic;

namespace DynaApp.Entities
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
    }
}
