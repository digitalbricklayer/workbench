using System;
using System.Collections;
using System.Collections.Generic;

namespace Workbench.Core.Solvers
{
    internal sealed class ConstraintNetwork
    {
        private readonly List<Arc> _arcs;

        internal ConstraintNetwork()
        {
            _arcs = new List<Arc>();
        }

        internal bool IsSolved
        {
            get { return _arcs.TrueForAll(arc => arc.IsSolved); }
        }

        internal Arc[] ToArray()
        {
            return _arcs.ToArray();
        }

        internal void AddArc(Arc arc)
        {
            _arcs.Add(arc);
        }

        internal IEnumerable<IntegerVariable> GetSingletonVariables()
        {
            var variables = new HashSet<IntegerVariable>(new IntegerVariableComparer());
            foreach (var arc in _arcs)
            {
                variables.Add(arc.Left);
                variables.Add(arc.Right);
            }
            return variables;
        }
    }

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

    internal sealed class Arc
    {
        internal Arc(IntegerVariable left, IntegerVariable right, ConstraintExpression constraint)
        {
            Left = left;
            Right = right;
            Constraint = constraint;
        }

        internal IntegerVariable Left { get; }

        internal IntegerVariable Right { get; }

        internal ConstraintExpression Constraint { get; }
        internal bool IsSolved => !Left.Domain.IsEmpty && !Right.Domain.IsEmpty;
    }
}
