using System;
using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Compares integer variables using the name as the referential attribute.
    /// </summary>
    internal class IntegerVariableComparer : IEqualityComparer<IntegerVariable>
    {
        public bool Equals(IntegerVariable x, IntegerVariable y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));
            return x.Name == y.Name;
        }

        public int GetHashCode(IntegerVariable obj)
        {
            return obj.GetHashCode();
        }
    }
}