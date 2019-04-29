using System;
using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Compares integer variables using the name as the referential attribute.
    /// </summary>
    internal class IntegerVariableComparer : IEqualityComparer<SolverVariable>
    {
        public bool Equals(SolverVariable x, SolverVariable y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            return x.Name == y.Name;
        }

        public int GetHashCode(SolverVariable obj)
        {
            return obj.GetHashCode();
        }
    }
}