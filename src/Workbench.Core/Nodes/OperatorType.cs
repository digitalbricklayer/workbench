using System;

namespace Workbench.Core.Nodes
{
    public static class OperatorTypeParser
    {
        public static OperatorType ParseOperatorFrom(string operatorToken)
        {
            switch (operatorToken)
            {
                case "<":
                    return OperatorType.Less;

                case "<=":
                    return OperatorType.LessThanOrEqual;

                case ">":
                    return OperatorType.Greater;

                case ">=":
                    return OperatorType.GreaterThanOrEqual;

                case "=":
                    return OperatorType.Equals;

                case "!=":
                case "<>":
                    return OperatorType.NotEqual;

                default:
                    throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Operator type in a binary expression.
    /// </summary>
    public enum OperatorType
    {
        Equals,
        NotEqual,
        Greater,
        Less,
        GreaterThanOrEqual,
        LessThanOrEqual
    }
}