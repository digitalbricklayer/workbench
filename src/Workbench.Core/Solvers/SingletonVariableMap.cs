using System;
using System.Diagnostics.Contracts;
using Google.OrTools.ConstraintSolver;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class SingletonVariableMap
    {
        internal SingletonVariableMap(SingletonVariableModel modelVariable, IntVar solverVariable)
        {
            Contract.Requires<ArgumentNullException>(modelVariable != null);
            SolverVariable = solverVariable;
            ModelVariable = modelVariable;
        }

        internal IntVar SolverVariable { get; }
        internal SingletonVariableModel ModelVariable { get; }
    }
}