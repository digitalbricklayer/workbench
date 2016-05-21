using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for visualizer binding expressions.
    /// </summary>
    [Language("Visualizer Binding", "0.1", "A grammar for binding a model solution into a visualizer.")]
    internal class VisualizerBindingGrammar : Grammar
    {
        private const string CounterRegexPattern = @"\b[A-Za-z]\w*\b";
        private const string VariableRegexPattern = @"\b[A-Za-z]\w*\b";
        private const string VisualizerNameRegexPattern = @"\b[A-Za-z]\w*\b";
        private const string VisualizerArgumentRegexPattern = @"\b[A-Za-z]\w*\b";

        public VisualizerBindingGrammar()
            : base(caseSensitive: false)
        {
            LanguageFlags = LanguageFlags.CreateAst;

            var EQUALS = ToTerm("=", "equal");
            var NOT_EQUAL = ToTerm("<>", "not equal");
            var ALT_NOT_EQUAL = ToTerm("!=", "alternative not equal");
            var GREATER = ToTerm(">", "greater");
            var GREATER_EQUAL = ToTerm(">=", "greater or equal");
            var LESS = ToTerm("<", "less");
            var LESS_EQUAL = ToTerm("<=", "less or equal");
            var BRACKET_OPEN = ToTerm("[");
            var BRACKET_CLOSE = ToTerm("]");
            var PARENTHESIS_OPEN = ToTerm("(");
            var PARENTHESIS_CLOSE = ToTerm(")");
            var PLUS = ToTerm("+");
            var MINUS = ToTerm("-");
            var COUNTER_SEPERATOR = ToTerm(",");
            var RANGE_SEPERATOR = ToTerm(",");
            var CALL_ARGUMENT_SEPERATOR = ToTerm(",");
            var CALL_ARGUMENT_VALUE_SEPERATOR = ToTerm(":");
            var IF = ToTerm("if");
            var IF_CONDITION_TERMINATOR = ToTerm(":-");

            var literal = new NumberLiteral("literal", NumberOptions.IntOnly, typeof(LiteralNode));
            var subscript = new NumberLiteral("subscript", NumberOptions.IntOnly, typeof(SubscriptNode));
            var variableName = new RegexBasedTerminal("variable", VariableRegexPattern);
            variableName.AstConfig.NodeType = typeof(VariableNameNode);
            var counterReference = new RegexBasedTerminal("counter reference", CounterRegexPattern);
            counterReference.AstConfig.NodeType = typeof(CounterReferenceNode);
            var visualizerNameReference = new RegexBasedTerminal("visualizer reference", VisualizerNameRegexPattern);
            visualizerNameReference.AstConfig.NodeType = typeof(VisualizerNameReferenceNode);

            var ifStatement = new NonTerminal("if", typeof(IfStatementNode));
            var statement = new NonTerminal("statement", typeof(StatementNode));
            var bindingExpression = new NonTerminal("binding expression", typeof(VisualizerExpressionNode));

            var infixStatement = new NonTerminal("infix statement", typeof(InfixStatementNode));
            infixStatement.Rule = literal | counterReference;
            var infixOperators = new NonTerminal("infix");
            infixOperators.Rule = PLUS | MINUS;
            var subscriptStatement = new NonTerminal("subscript statement", typeof(SubscriptStatementNode));
            subscriptStatement.Rule = subscript | counterReference;
            var aggregateVariableReference = new NonTerminal("aggregateVariableReference", typeof(AggregateVariableReferenceNode));
            aggregateVariableReference.Rule = variableName + BRACKET_OPEN + subscriptStatement + BRACKET_CLOSE;

            var aggregateVariableReferenceExpression = new NonTerminal("aggregate expression", typeof(AggregateVariableReferenceExpressionNode));
            aggregateVariableReferenceExpression.Rule = aggregateVariableReference + infixOperators + infixStatement;

            var singletonVariableReference = new NonTerminal("singletonVariableReference", typeof(SingletonVariableReferenceNode));
            singletonVariableReference.Rule = variableName;

            var singletonVariableReferenceExpression = new NonTerminal("singleton expression", typeof(SingletonVariableReferenceExpressionNode));
            singletonVariableReferenceExpression.Rule = singletonVariableReference + infixOperators + infixStatement;

            var callArgumentName = new RegexBasedTerminal("call argument name", VisualizerArgumentRegexPattern);
            callArgumentName.AstConfig.NodeType = typeof(CallArgumentNameNode);

            var callArgumentStringValue = new RegexBasedTerminal("call argument value string", VisualizerArgumentRegexPattern);
            callArgumentStringValue.AstConfig.NodeType = typeof(CallArgumentStringValueNode);

            var callArgumentNumberValue = new NumberLiteral("call argument value number",
                                                            NumberOptions.IntOnly,
                                                            typeof(CallArgumentNumberValueNode));

            var callArgumentValue = new NonTerminal("call argument value", typeof(CallArgumentValueNode));
            callArgumentValue.Rule = callArgumentNumberValue | callArgumentStringValue;

            var callArgument = new NonTerminal("call argument", typeof(CallArgumentNode));
            callArgument.Rule = callArgumentName + CALL_ARGUMENT_VALUE_SEPERATOR + callArgumentValue;

            var callArgumentList = new NonTerminal("call argument list", typeof(CallArgumentNodeList));
            callArgumentList.Rule = MakePlusRule(callArgumentList, CALL_ARGUMENT_SEPERATOR, callArgument);

            var callStatement = new NonTerminal("call statement", typeof(CallStatementNode));
            callStatement.Rule = visualizerNameReference + PARENTHESIS_OPEN + callArgumentList + PARENTHESIS_CLOSE;

            var binaryOperators = new NonTerminal("binary operators", "operator");
            binaryOperators.Rule = EQUALS |
                                   NOT_EQUAL | ALT_NOT_EQUAL |
                                   LESS | LESS_EQUAL |
                                   GREATER | GREATER_EQUAL;
            var expression = new NonTerminal("expression", typeof(ExpressionNode));
            expression.Rule = aggregateVariableReference | aggregateVariableReferenceExpression |
                              singletonVariableReference | singletonVariableReferenceExpression |
                              literal;

            var counterDeclaration = new RegexBasedTerminal("counter declaration", CounterRegexPattern);
            counterDeclaration.AstConfig.NodeType = typeof(CounterDeclarationNode);

            var scopeLimitStatement = new NonTerminal("scope limit statement", typeof(ScopeLimitSatementNode));
            scopeLimitStatement.Rule = literal | counterReference;

            var expanderCountStatement = new NonTerminal("expander counter", typeof(ExpanderCountNode));
            expanderCountStatement.Rule = literal | counterReference;

            var scopeStatement = new NonTerminal("scope", typeof(ScopeStatementNode));
            scopeStatement.Rule = scopeLimitStatement + ToTerm("..") + scopeLimitStatement;

            var expanderScopeStatement = new NonTerminal("expander scope", typeof(ExpanderScopeNode));
            expanderScopeStatement.Rule = scopeStatement | expanderCountStatement;

            var multiCounterDeclaration = new NonTerminal("counters", typeof(MultiCounterDeclarationNode));
            multiCounterDeclaration.Rule = MakePlusRule(multiCounterDeclaration, COUNTER_SEPERATOR, counterDeclaration);

            var multiExpanderScopeStatement = new NonTerminal("scopes", typeof(MultiScopeDeclarationNode));
            multiExpanderScopeStatement.Rule = MakePlusRule(multiExpanderScopeStatement, RANGE_SEPERATOR, expanderScopeStatement);

            var expanderStatementList = new NonTerminal("multi-expander", typeof(MultiRepeaterStatementNode));
            expanderStatementList.Rule = multiCounterDeclaration + ToTerm("in") + multiExpanderScopeStatement + ToTerm(":-") + ifStatement;

            var binaryExpression = new NonTerminal("binary expression", typeof(BinaryExpressionNode));
            binaryExpression.Rule = expression + binaryOperators + expression;

            statement.Rule = ifStatement;
            ifStatement.Rule = IF + binaryExpression + IF_CONDITION_TERMINATOR + callStatement;

            bindingExpression.Rule = expanderStatementList /*| ifStatement | callStatement*/ | Empty;

            Root = bindingExpression;

            MarkTransient(binaryOperators, infixOperators);
//            MarkPunctuation(PARENTHESIS_OPEN, PARENTHESIS_CLOSE);
//            RegisterBracePair("(", ")");
        }
    }
}
