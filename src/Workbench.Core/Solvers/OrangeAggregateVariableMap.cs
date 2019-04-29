using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Map aggregate variables in the model to their equivalent in the solver representation.
    /// </summary>
    internal sealed class OrangeAggregateVariableMap
    {
        internal AggregateVariableModel ModelVariable { get;  }
        internal AggregateSolverVariable SolverVariable { get; }

        internal OrangeAggregateVariableMap(AggregateVariableModel modelVariable, AggregateSolverVariable solverVariable)
        {
            ModelVariable = modelVariable;
            SolverVariable = solverVariable;
        }

        internal SolverVariable GetAt(int index)
        {
            return SolverVariable.GetAt(index);
        }
    }
}