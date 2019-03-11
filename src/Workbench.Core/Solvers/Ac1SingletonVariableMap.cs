using System;
using System.Diagnostics.Contracts;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Map singleton variables in the model to their equivalent in the solver representation.
    /// </summary>
    internal sealed class Ac1SingletonVariableMap
    {
        internal Ac1SingletonVariableMap(SingletonVariableModel modelVariable, IntegerVariable solverVariable)
        {
            Contract.Requires<ArgumentNullException>(modelVariable != null);
            Contract.Requires<ArgumentNullException>(solverVariable != null);
            SolverVariable = solverVariable;
            ModelVariable = modelVariable;
        }

        internal IntegerVariable SolverVariable { get; }
        internal SingletonVariableModel ModelVariable { get; }
    }
}