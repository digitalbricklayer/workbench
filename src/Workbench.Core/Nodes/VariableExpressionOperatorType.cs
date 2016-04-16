namespace Workbench.Core.Nodes
{
    public static class VariableExpressionOperatorParser
    {
        public static VariableExpressionOperatorType ParseFrom(string operatorToken)
        {
            switch (operatorToken)
            {
                case "+":
                    return VariableExpressionOperatorType.Add;

                case "-":
                    return VariableExpressionOperatorType.Minus;

                default:
                    throw new System.NotImplementedException();
            }
        }
    }

    /// <summary>
    /// Operator type in a variable expression.
    /// </summary>
    public enum VariableExpressionOperatorType
    {
        Add,
        Minus
    }
}