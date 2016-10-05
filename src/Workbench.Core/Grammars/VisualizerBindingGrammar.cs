using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for visualizer binding expressions.
    /// </summary>
    [Language("Visualizer Binding", "0.1", "A grammar for binding a model solution to a visualizer.")]
    internal class VisualizerBindingGrammar : Grammar
    {
        private const string CounterRegexPattern = @"\b[A-Za-z]\w*\b";
        private const string VariableRegexPattern = @"\b[A-Za-z]\w*\b";
        private const string VisualizerNameRegexPattern = @"\b[A-Za-z]\w*\b";
        private const string VisualizerArgumentRegexPattern = @"\b[A-Za-z]\w*\b";

        public VisualizerBindingGrammar()
            : base(caseSensitive: false)
        {
            LanguageFlags = LanguageFlags.CreateAst |
                            LanguageFlags.NewLineBeforeEOF;

            var EQUALS = ToTerm("=", "equal");
            var NOT_EQUAL = ToTerm("<>", "not equal");
            var ALT_NOT_EQUAL = ToTerm("!=", "alternative not equal");
            var GREATER = ToTerm(">", "greater");
            var GREATER_EQUAL = ToTerm(">=", "greater or equal");
            var LESS = ToTerm("<", "less");
            var LESS_EQUAL = ToTerm("<=", "less or equal");
            var PARENTHESIS_OPEN = ToTerm("(");
            var PARENTHESIS_CLOSE = ToTerm(")");
            var PLUS = ToTerm("+");
            var MINUS = ToTerm("-");
            var COMMA = ToTerm(",", "comma");
            var IF = ToTerm("if");
            var COLON = ToTerm(":", "colon");
            var FOR = ToTerm("for");
            var IN = ToTerm("in");

            // Terminals
            var visualizerNameReference = new RegexBasedTerminal("visualizer reference", VisualizerNameRegexPattern);
            visualizerNameReference.AstConfig.NodeType = typeof(VisualizerNameReferenceNode);
            var literal = new NumberLiteral("literal", NumberOptions.IntOnly, typeof(LiteralNode));
            var variableName = new RegexBasedTerminal("variable", VariableRegexPattern);
            variableName.AstConfig.NodeType = typeof(VariableNameNode);
            var counterReference = new RegexBasedTerminal("counter reference", CounterRegexPattern);
            counterReference.AstConfig.NodeType = typeof(CounterReferenceNode);
            var counterDeclaration = new RegexBasedTerminal("counter declaration", CounterRegexPattern);
            counterDeclaration.AstConfig.NodeType = typeof(CounterDeclarationNode);
            var callArgumentNumberValue = new NumberLiteral("call argument value number",
                                                            NumberOptions.IntOnly,
                                                            typeof(CallArgumentNumberValueNode));
            var callArgumentStringValue = new RegexBasedTerminal("call argument value string", VisualizerArgumentRegexPattern);
            callArgumentStringValue.AstConfig.NodeType = typeof(CallArgumentStringValueNode);
            var callArgumentName = new RegexBasedTerminal("call argument name", VisualizerArgumentRegexPattern);
            callArgumentName.AstConfig.NodeType = typeof(CallArgumentNameNode);

            // Non-terminals
            var ifStatement = new NonTerminal("if", typeof(IfStatementNode));
            var statement = new NonTerminal("statement", typeof(StatementNode));
            var bindingExpression = new NonTerminal("binding expression", typeof(VisualizerBindingExpressionNode));
            var infixStatement = new NonTerminal("infix statement", typeof(InfixStatementNode));

            var valueReferenceStatement = new NonTerminal("binary expression", typeof(ValueReferenceStatementNode));
            var valueOffset = new NonTerminal("offset", typeof(ValueOffsetNode));

            var binaryOperator = new NonTerminal("binary operators", "operator");
            var infixOperator = new NonTerminal("infix");
            var callArgumentValue = new NonTerminal("call argument value", typeof(CallArgumentValueNode));
            var callArgument = new NonTerminal("call argument", typeof(CallArgumentNode));
            var callArgumentList = new NonTerminal("call argument list", typeof(CallArgumentNodeList));
            var callStatement = new NonTerminal("call statement", typeof(CallStatementNode));
            var expression = new NonTerminal("expression", typeof(VisualizerExpressionNode));
            var scopeLimitStatement = new NonTerminal("scope limit statement", typeof(ScopeLimitSatementNode));
            var expanderCountStatement = new NonTerminal("expander counter", typeof(ExpanderCountNode));
            var scopeStatement = new NonTerminal("scope", typeof(ScopeStatementNode));
            var expanderScopeStatement = new NonTerminal("expander scope", typeof(ExpanderScopeNode));
            var counterDeclarationList = new NonTerminal("counters", typeof(MultiCounterDeclarationNode));
            var expanderStatement = new NonTerminal("multi-expander", typeof(MultiRepeaterStatementNode));
            var expanderScopeStatementList = new NonTerminal("scopes", typeof(MultiScopeDeclarationNode));
            var binaryExpression = new NonTerminal("binary expression", typeof(VisualizerBinaryExpressionNode));

            // BNF rules
            infixStatement.Rule = literal | counterReference;
            infixOperator.Rule = PLUS | MINUS;

            valueOffset.Rule = literal | counterReference;
            valueReferenceStatement.Rule = ToTerm("<") + variableName + COMMA + valueOffset + ToTerm(">") |
                                           ToTerm("<") + variableName + ToTerm(">");

            callArgumentValue.Rule = callArgumentNumberValue | callArgumentStringValue;
            callArgument.Rule = callArgumentName + COLON + callArgumentValue;
            callArgumentList.Rule = MakePlusRule(callArgumentList, COMMA, callArgument);
            callStatement.Rule = visualizerNameReference + PARENTHESIS_OPEN + callArgumentList + PARENTHESIS_CLOSE;

            binaryOperator.Rule = EQUALS |
                                  NOT_EQUAL | ALT_NOT_EQUAL |
                                  LESS | LESS_EQUAL |
                                  GREATER | GREATER_EQUAL;
            expression.Rule = valueReferenceStatement | literal | counterReference;
            binaryExpression.Rule = expression + binaryOperator + expression;
            scopeLimitStatement.Rule = literal | counterReference;
            expanderCountStatement.Rule = literal | counterReference;
            scopeStatement.Rule = scopeLimitStatement + ToTerm("..") + scopeLimitStatement;
            expanderScopeStatement.Rule = scopeStatement | expanderCountStatement;
            counterDeclarationList.Rule = MakePlusRule(counterDeclarationList, COMMA, counterDeclaration);
            expanderScopeStatementList.Rule = MakePlusRule(expanderScopeStatementList, COMMA, expanderScopeStatement);
            expanderStatement.Rule = FOR + counterDeclarationList + IN + expanderScopeStatementList + COLON + statement;

            ifStatement.Rule = IF + binaryExpression + COLON + callStatement;
            statement.Rule = ifStatement | callStatement;

            bindingExpression.Rule = NewLine |
                                     statement + NewLine |
                                     expanderStatement + NewLine;

            Root = bindingExpression;

            // Operator precedence
            RegisterOperators(1, PLUS, MINUS);

            // Punctuation and transient terms
            MarkReservedWords("for", "if", "in");
            RegisterBracePair("(", ")");
            RegisterBracePair("<", ">");
            MarkTransient(binaryOperator, infixOperator);
            MarkPunctuation(PARENTHESIS_OPEN, PARENTHESIS_CLOSE);
            MarkPunctuation(FOR, IF, COLON, COMMA);
            MarkPunctuation("<", ">");
        }
    }
}
