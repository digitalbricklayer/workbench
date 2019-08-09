using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class OrSingletonVariableMap
    {
        internal OrSingletonVariableMap(SingletonVariableModel modelVariable, IntVar solverVariable)
        {
            SolverVariable = solverVariable;
            ModelVariable = modelVariable;
        }

        internal IntVar SolverVariable { get; }
        internal SingletonVariableModel ModelVariable { get; }
    }
}