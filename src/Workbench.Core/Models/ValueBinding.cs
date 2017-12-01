using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Binding between a model value and the underlying value used by the solver.
    /// </summary>
	/// <remarks>
	/// When the value is for an integer domain, the model and solver values will be the same.
	/// </remarks>
    [Serializable]
    public sealed class ValueBinding
    {
        /// <summary>
        /// Initialize a value binding with a model value and a solver value.
        /// </summary>
        /// <param name="theModelValue">Model value.</param>
        /// <param name="theSolverValue">Solver value.</param>
        public ValueBinding(object theModelValue, long theSolverValue)
        {
            Contract.Requires<ArgumentNullException>(theModelValue != null);

            Model = theModelValue;
            Solver = theSolverValue;
        }

        /// <summary>
        /// Gets the model value.
        /// </summary>
        public object Model { get; private set; }

        /// <summary>
        /// Gets the solver value.
        /// </summary>
        public long Solver { get; private set; }
    }
}
