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

            var EQUALS = ToTerm("=", "equal");
            var NOT_EQUAL = ToTerm("<>", "not equal");
            var GREATER = ToTerm(">", "greater");
            var GREATER_EQUAL = ToTerm(">=", "greater or equal");
            var LESS = ToTerm("<", "less");
            var LESS_EQUAL = ToTerm("<=", "less or equal");
            var BRACKET_OPEN = ToTerm("[");
            var BRACKET_CLOSE = ToTerm("]");

            var literal = new NumberLiteral("literal", NumberOptions.IntOnly, typeof(LiteralNode));
            var subscript = new NumberLiteral("subscript", NumberOptions.IntOnly, typeof(SubscriptNode));
            var variableName = new RegexBasedTerminal("variable", @"\b[A-Za-z]\w*\b");
            variableName.AstConfig.NodeType = typeof (VariableNameNode);
            var aggregateVariableReference = new NonTerminal("aggregateVariableReference", typeof(AggregateVariableReferenceNode));
            aggregateVariableReference.Rule = variableName + BRACKET_OPEN + subscript + BRACKET_CLOSE;
            var singletonVariableReference = new NonTerminal("singletonVariableReference", typeof(SingletonVariableReferenceNode));
            singletonVariableReference.Rule = variableName;
 
            var equal = new NonTerminal("equal", typeof(BinaryExpressionNode));
            var notEqual = new NonTerminal("not equal", typeof(BinaryExpressionNode));
            var greaterThan = new NonTerminal("greater", typeof(BinaryExpressionNode));
            var greaterThanOrEqual = new NonTerminal("greater than or equal", typeof(BinaryExpressionNode));
            var lessThan = new NonTerminal("less", typeof(BinaryExpressionNode));
            var lessThanOrEqual = new NonTerminal("less than or equal", typeof(BinaryExpressionNode));
            var expression = new NonTerminal("expression", typeof(ExpressionNode));
            expression.Rule = aggregateVariableReference |
                              singletonVariableReference |
                              literal;

            equal.Rule = expression + EQUALS + expression;
            notEqual.Rule = expression + NOT_EQUAL + expression;
            greaterThan.Rule = expression + GREATER + expression;
            greaterThanOrEqual.Rule = expression + GREATER_EQUAL + expression;
            lessThan.Rule = expression + LESS + expression;
            lessThanOrEqual.Rule = expression + LESS_EQUAL + expression;

            var constraintExpression = new NonTerminal("constraint expression", typeof(ConstraintExpressionNode));
            constraintExpression.Rule = equal | 
                                        notEqual | 
                                        greaterThan | 
                                        greaterThanOrEqual | 
                                        lessThan | 
                                        lessThanOrEqual;
            this.Root = constraintExpression;
        }
    }
}
