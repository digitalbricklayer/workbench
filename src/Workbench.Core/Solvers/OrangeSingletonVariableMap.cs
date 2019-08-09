using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Map singleton variables in the model to their equivalent in the solver representation.
    /// </summary>
    internal sealed class OrangeSingletonVariableMap
    {
        internal OrangeSingletonVariableMap(SingletonVariableModel modelVariable, SolverVariable solverVariable)
        {
            SolverVariable = solverVariable;
            ModelVariable = modelVariable;
        }

        internal SolverVariable SolverVariable { get; }
        internal SingletonVariableModel ModelVariable { get; }
    }
}