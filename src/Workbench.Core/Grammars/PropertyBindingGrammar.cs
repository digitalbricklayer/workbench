using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for visualizer property binding expressions.
    /// </summary>
    [Language("Property Binding Expression", "0.1", "A grammar for binding a property inside a visualizer.")]
    internal class PropertyBindingGrammar : Grammar
    {
        internal PropertyBindingGrammar()
            : base(caseSensitive: false)
        {
            LanguageFlags = LanguageFlags.CreateAst |
                            LanguageFlags.NewLineBeforeEOF;

            // Terms
            var EQUALS = ToTerm("=", "equal");
            var NOT_EQUAL = ToTerm("<>", "not equal");
            var ALT_NOT_EQUAL = ToTerm("!=", "alternative not equal");
            var GREATER = ToTerm(">", "greater");
            var GREATER_EQUAL = ToTerm(">=", "greater or equal");
            var LESS = ToTerm("<", "less");
            var LESS_EQUAL = ToTerm("<=", "less or equal");
            var PLUS = ToTerm("+");
            var MINUS = ToTerm("-");
            var COMMA = ToTerm(",", "comma");
            var IF = ToTerm("if");
            var COLON = ToTerm(":", "colon");

            // Terminals
            var numberLiteral = new NumberLiteral("literal", NumberOptions.IntOnly, typeof(IntegerLiteralNode));
            var characterLiteral = new StringLiteral("character literal", "'", StringOptions.IsChar);
            characterLiteral.AstConfig.NodeType = typeof(CharacterLiteralNode);
            var variableName = new IdentifierTerminal("variable");
            variableName.AstConfig.NodeType = typeof(VariableNameNode);

            // Non-terminals
            var ifStatement = new NonTerminal("if", typeof(IfStatementNode));
            var statement = new NonTerminal("statement", typeof(StatementNode));
            var bindingExpression = new NonTerminal("binding expression", typeof(PropertyUpdateExpressionNode));
            var infixStatement = new NonTerminal("infix statement", typeof(InfixStatementNode));

            var valueReferenceStatement = new NonTerminal("binary expression", typeof(ValueReferenceStatementNode));
            var valueOffset = new NonTerminal("offset", typeof(ValueOffsetNode));

            var binaryOperator = new NonTerminal("binary operators", "operator");
            var infixOperator = new NonTerminal("infix");
            var expression = new NonTerminal("expression", typeof(VisualizerExpressionNode));
            var binaryExpression = new NonTerminal("binary expression", typeof(VisualizerBinaryExpressionNode));

            // BNF rules
            infixStatement.Rule = numberLiteral;
            infixOperator.Rule = PLUS | MINUS;

            valueOffset.Rule = numberLiteral;
            // A value reference can either reference a singleton or one element of an aggregate
            valueReferenceStatement.Rule = ToTerm("<") + variableName + COMMA + valueOffset + ToTerm(">") |
                                           ToTerm("<") + variableName + ToTerm(">");

            binaryOperator.Rule = EQUALS |
                                  NOT_EQUAL | ALT_NOT_EQUAL |
                                  LESS | LESS_EQUAL |
                                  GREATER | GREATER_EQUAL;

            expression.Rule = valueReferenceStatement |
                              numberLiteral |
                              characterLiteral;
            binaryExpression.Rule = expression + binaryOperator + expression;

            ifStatement.Rule = IF + binaryExpression + COLON + expression;
            statement.Rule = ifStatement | expression;

            bindingExpression.Rule = NewLine | statement + NewLine;

            Root = bindingExpression;

            // Operator precedence
            RegisterOperators(1, PLUS, MINUS);

            // Punctuation and transient terms
            MarkReservedWords("if");
            RegisterBracePair("<", ">");
            MarkTransient(binaryOperator, infixOperator);
            MarkPunctuation(IF, COLON, COMMA);
            MarkPunctuation("<", ">");
        }
    }
}
