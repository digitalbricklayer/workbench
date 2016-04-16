using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for constraint expressions.
    /// </summary>
    [Language("Constraint Expression", "0.1", "A grammar for expressing constraints.")]
    internal class ConstraintGrammar : Grammar
    {
        public ConstraintGrammar()
            : base(caseSensitive:false)
        {
            LanguageFlags = LanguageFlags.CreateAst;

            var literal = new NumberLiteral("literal", NumberOptions.IntOnly, typeof(LiteralNode));
            var subscript = new NumberLiteral("subscript", NumberOptions.IntOnly, typeof(SubscriptNode));
            var variableName = new RegexBasedTerminal("variable", @"\b[A-Za-z]\w*\b");
            variableName.AstConfig.NodeType = typeof(VariableNameNode);
            var EQUALS = ToTerm("=", "equal");
            var NOT_EQUAL = ToTerm("<>", "not equal");
            var GREATER = ToTerm(">", "greater");
            var GREATER_EQUAL = ToTerm(">=", "greater or equal");
            var LESS = ToTerm("<", "less");
            var LESS_EQUAL = ToTerm("<=", "less or equal");
            var BRACKET_OPEN = ToTerm("[");
            var BRACKET_CLOSE = ToTerm("]");

            var aggregateVariableReference = new NonTerminal("aggregateVariableReference", typeof(AggregateVariableReferenceNode));
            aggregateVariableReference.Rule = variableName + BRACKET_OPEN + subscript + BRACKET_CLOSE;
            var singletonVariableReference = new NonTerminal("singletonVariableReference", typeof(SingletonVariableReferenceNode));
            singletonVariableReference.Rule = variableName;

            var binaryOperators = new NonTerminal("binary operators", "operator");
            binaryOperators.Rule = EQUALS |
                                   NOT_EQUAL |
                                   LESS |
                                   LESS_EQUAL |
                                   GREATER |
                                   GREATER_EQUAL;
            var expression = new NonTerminal("expression", typeof(ExpressionNode));
            expression.Rule = aggregateVariableReference |
                              singletonVariableReference |
                              literal;

            var binaryExpression = new NonTerminal("binary expression", typeof(BinaryExpressionNode));
            binaryExpression.Rule = expression + binaryOperators + expression;

            var constraintExpression = new NonTerminal("constraint expression", typeof(ConstraintExpressionNode));
            constraintExpression.Rule = binaryExpression;
            this.Root = constraintExpression;

            MarkTransient(binaryOperators);
        }
    }
}
