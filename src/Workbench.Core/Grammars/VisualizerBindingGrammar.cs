using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for visualizer binding expressions.
    /// </summary>
    [Language("Visualizer Binding Expression", "0.1", "A grammar for binding a model solution to a visualizer.")]
    internal class VisualizerBindingGrammar : Grammar
    {
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
            var RANGE = ToTerm("..", "range");
            var SIZE_FUNC = ToTerm("size", "size function");

            // Terminals
            var visualizerNameReference = new IdentifierTerminal("visualizer reference");
            visualizerNameReference.AstConfig.NodeType = typeof(VisualizerNameReferenceNode);
            var literal = new NumberLiteral("literal", NumberOptions.IntOnly, typeof(LiteralNode));
            var variableName = new IdentifierTerminal("variable");
            variableName.AstConfig.NodeType = typeof(VariableNameNode);
            var counterReference = new IdentifierTerminal("counter reference");
            counterReference.AstConfig.NodeType = typeof(CounterReferenceNode);
            var counterDeclaration = new IdentifierTerminal("counter declaration");
            counterDeclaration.AstConfig.NodeType = typeof(CounterDeclarationNode);
            var callArgumentNumberValue = new NumberLiteral("call argument value number", NumberOptions.IntOnly, typeof(CallArgumentNumberValueNode));
            var callArgumentStringValue = new IdentifierTerminal("call argument value string");
            callArgumentStringValue.AstConfig.NodeType = typeof(CallArgumentStringValueNode);
            var callArgumentName = new IdentifierTerminal("call argument name");
            callArgumentName.AstConfig.NodeType = typeof(CallArgumentNameNode);
            var variableReference = new IdentifierTerminal("variable reference", IdOptions.IsNotKeyword);
            variableReference.AstConfig.NodeType = typeof(FunctionCallArgumentStringLiteralNode);

            // Non-terminals
            var functionName = new NonTerminal("function name", typeof(FunctionNameNode));
            var functionInvocation = new NonTerminal("function call", typeof(FunctionInvocationNode));
            var functionArgumentList = new NonTerminal("function arguments", typeof(FunctionArgumentListNode));
            var functionArgument = new NonTerminal("function argument", typeof(FunctionCallArgumentNode));

            var ifStatement = new NonTerminal("if", typeof(IfStatementNode));
            var statement = new NonTerminal("statement", typeof(StatementNode));
            var statementList = new NonTerminal("statement list", typeof(StatementListNode));
            var bindingExpression = new NonTerminal("binding expression", typeof(VisualizerBindingExpressionNode));
            var infixStatement = new NonTerminal("infix statement", typeof(InfixStatementNode));

            var valueReferenceStatement = new NonTerminal("binary expression", typeof(ValueReferenceStatementNode));
            var valueOffset = new NonTerminal("offset", typeof(ValueOffsetNode));

            var binaryOperator = new NonTerminal("binary operators", "operator");
            var infixOperator = new NonTerminal("infix");
            var callArgumentValue = new NonTerminal("call argument value", typeof(CallArgumentValueNode));
            var callArgument = new NonTerminal("call argument", typeof(CallArgumentNode));
            var callArgumentList = new NonTerminal("call argument list", typeof(CallArgumentListNode));
            var callStatement = new NonTerminal("call statement", typeof(CallStatementNode));
            var expression = new NonTerminal("expression", typeof(VisualizerExpressionNode));
            var scopeLimitStatement = new NonTerminal("scope limit statement", typeof(ScopeLimitSatementNode));
            var expanderCountStatement = new NonTerminal("expander counter", typeof(ExpanderCountNode));
            var scopeStatement = new NonTerminal("scope", typeof(ScopeStatementNode));
            var expanderScopeStatement = new NonTerminal("expander scope", typeof(ExpanderScopeNode));
            var counterDeclarationList = new NonTerminal("counters", typeof(CounterDeclarationListNode));
            var expanderStatement = new NonTerminal("multi-expander", typeof(MultiRepeaterStatementNode));
            var expanderScopeStatementList = new NonTerminal("scopes", typeof(ScopeDeclarationListNode));
            var binaryExpression = new NonTerminal("binary expression", typeof(VisualizerBinaryExpressionNode));

            // BNF rules
            functionName.Rule = SIZE_FUNC;
            functionArgument.Rule = variableReference;
            functionArgumentList.Rule = MakePlusRule(functionArgumentList, COMMA, functionArgument);
            functionInvocation.Rule = functionName + PARENTHESIS_OPEN + functionArgumentList + PARENTHESIS_CLOSE;

            infixStatement.Rule = literal | counterReference;
            infixOperator.Rule = PLUS | MINUS;

            valueOffset.Rule = literal | counterReference;
            // A value reference can either reference a singleton or an aggregate
            valueReferenceStatement.Rule = ToTerm("<") + variableName + COMMA + valueOffset + ToTerm(">") |
                                           ToTerm("<") + variableName + ToTerm(">");

            callArgumentValue.Rule =  valueReferenceStatement | callArgumentNumberValue | callArgumentStringValue;
            callArgument.Rule = callArgumentName + COLON + callArgumentValue;
            callArgumentList.Rule = MakePlusRule(callArgumentList, COMMA, callArgument);
            callStatement.Rule = visualizerNameReference + PARENTHESIS_OPEN + callArgumentList + PARENTHESIS_CLOSE;

            binaryOperator.Rule = EQUALS |
                                  NOT_EQUAL | ALT_NOT_EQUAL |
                                  LESS | LESS_EQUAL |
                                  GREATER | GREATER_EQUAL;
            expression.Rule = valueReferenceStatement | literal | counterReference;
            binaryExpression.Rule = expression + binaryOperator + expression;
            scopeLimitStatement.Rule = literal | counterReference | functionInvocation;
            expanderCountStatement.Rule = literal | counterReference | functionInvocation;
            scopeStatement.Rule = scopeLimitStatement + RANGE + scopeLimitStatement;
            expanderScopeStatement.Rule = scopeStatement | expanderCountStatement;
            counterDeclarationList.Rule = MakePlusRule(counterDeclarationList, COMMA, counterDeclaration);
            expanderScopeStatementList.Rule = MakePlusRule(expanderScopeStatementList, COMMA, expanderScopeStatement);
            expanderStatement.Rule = FOR + counterDeclarationList + IN + expanderScopeStatementList + COLON + statement;

            ifStatement.Rule = IF + binaryExpression + COLON + callStatement;
            statement.Rule = ifStatement | callStatement;
            statementList.Rule = MakePlusRule(statementList, COMMA, statement);

            bindingExpression.Rule = NewLine |
                                     statementList + NewLine |
                                     expanderStatement + NewLine;

            Root = bindingExpression;

            // Operator precedence
            RegisterOperators(1, PLUS, MINUS);

            // Punctuation and transient terms
            MarkReservedWords("for", "if", "in");
            RegisterBracePair("(", ")");
            RegisterBracePair("<", ">");
            MarkTransient(binaryOperator, infixOperator, functionName);
            MarkPunctuation(PARENTHESIS_OPEN, PARENTHESIS_CLOSE);
            MarkPunctuation(FOR, IF, COLON, COMMA, RANGE);
            MarkPunctuation("<", ">");
        }
    }
}
