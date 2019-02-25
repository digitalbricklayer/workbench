using System;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    internal sealed class ConstraintNetwork
    {
        internal Arc[] ToArray()
        {
            return Array.Empty<Arc>();
        }
    }

    internal sealed class Arc
    {
        internal Arc(SingletonVariableModel from, SingletonVariableModel to, ConstraintExpressionModel constraint)
        {
            From = from;
            To = to;
            Constraint = constraint;
        }

        internal SingletonVariableModel From { get; }
        internal SingletonVariableModel To { get; }
        internal ConstraintExpressionModel Constraint { get; }
    }
}
