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
        internal ConstraintGrammar()
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
            var BRACKET_OPEN = ToTerm("[");
            var BRACKET_CLOSE = ToTerm("]");
            var PLUS = ToTerm("+");
            var MINUS = ToTerm("-");
            var PIPE = ToTerm("|", "pipe");
            var COMMA = ToTerm(",", "seperator");
            var OPEN_ARG = ToTerm("(", "function call open args");
            var CLOSE_ARG = ToTerm(")", "function call close args");
            var SIZE_FUNC = ToTerm("size", "size function");
            var RANGE = ToTerm("..", "range");
            var IN = ToTerm("in");

            // Terminals
            var numberLiteral = new NumberLiteral("integer literal", NumberOptions.IntOnly, typeof(IntegerLiteralNode));
            var characterLiteral = new StringLiteral("character literal", "'", StringOptions.IsChar);
            characterLiteral.AstConfig.NodeType = typeof(CharacterLiteralNode);
            var subscript = new NumberLiteral("subscript", NumberOptions.IntOnly, typeof(SubscriptNode));
            var variableName = new IdentifierTerminal("variable name");
            variableName.AstConfig.NodeType = typeof(VariableNameNode);
            variableName.AddPrefix("$", IdOptions.IsNotKeyword);
            var counterReference = new IdentifierTerminal("counter reference");
            counterReference.AstConfig.NodeType = typeof(CounterReferenceNode);
            var counterDeclaration = new IdentifierTerminal("counter declaration");
            counterDeclaration.AstConfig.NodeType = typeof(CounterDeclarationNode);
            var variableReference = new IdentifierTerminal("variable reference", IdOptions.IsNotKeyword);
            variableReference.AstConfig.NodeType = typeof(FunctionCallArgumentStringLiteralNode);
            var itemName = new IdentifierTerminal("string literal", IdOptions.IsNotKeyword);
            itemName.AstConfig.NodeType = typeof(ItemNameNode);

            // Non-terminals
            var infixStatement = new NonTerminal("infix statement", typeof(InfixStatementNode));
            var infixOperators = new NonTerminal("infix");
            var subscriptStatement = new NonTerminal("subscript statement", typeof(SubscriptStatementNode));
            var aggregateVariableReference = new NonTerminal("aggregateVariableReference", typeof(AggregateVariableReferenceNode));
            var aggregateVariableReferenceExpression = new NonTerminal("aggregate expression", typeof(AggregateVariableReferenceExpressionNode));
            var singletonVariableReference = new NonTerminal("singletonVariableReference", typeof(SingletonVariableReferenceNode));
            var singletonVariableReferenceExpression = new NonTerminal("singleton expression", typeof(SingletonVariableReferenceExpressionNode));
            var binaryOperators = new NonTerminal("binary operators", "operator");
            var expression = new NonTerminal("expression", typeof(ExpressionNode));
            var scopeLimitStatement = new NonTerminal("scope limit statement", typeof(ScopeLimitSatementNode));
            var expanderCountStatement = new NonTerminal("expander counter", typeof(ExpanderCountNode));
            var scopeStatement = new NonTerminal("scope", typeof(ScopeStatementNode));
            var expanderScopeStatement = new NonTerminal("expander scope", typeof(ExpanderScopeNode));
            var multiCounterDeclaration = new NonTerminal("counters", typeof(CounterDeclarationListNode));
            var multiExpanderScopeStatement = new NonTerminal("scopes", typeof(ScopeDeclarationListNode));
            var multiExpanderStatement = new NonTerminal("multi-expander", typeof(MultiRepeaterStatementNode));
            var binaryExpression = new NonTerminal("binary expression", typeof(BinaryExpressionNode));
            var constraintExpression = new NonTerminal("constraint expression", typeof(ConstraintExpressionNode));
            var functionName = new NonTerminal("function name", typeof(FunctionNameNode));
            var functionInvocation = new NonTerminal("function call", typeof(FunctionInvocationNode));
            var functionArgumentList = new NonTerminal("function arguments", typeof(FunctionArgumentListNode));
            var functionArgument = new NonTerminal("function argument", typeof(FunctionCallArgumentNode));

            // BNF rules
            functionName.Rule = SIZE_FUNC;
            functionArgument.Rule = variableReference;
            functionArgumentList.Rule = MakePlusRule(functionArgumentList, COMMA, functionArgument);
            functionInvocation.Rule = functionName + OPEN_ARG + functionArgumentList + CLOSE_ARG;

            infixStatement.Rule = numberLiteral | counterReference;
            infixOperators.Rule = PLUS | MINUS;
            subscriptStatement.Rule = subscript | counterReference;
            aggregateVariableReference.Rule = variableName + BRACKET_OPEN + subscriptStatement + BRACKET_CLOSE;
            aggregateVariableReferenceExpression.Rule = aggregateVariableReference + infixOperators + infixStatement;
            singletonVariableReference.Rule = variableName;
            singletonVariableReferenceExpression.Rule = singletonVariableReference + infixOperators + infixStatement;

            binaryOperators.Rule = EQUALS |
                                   NOT_EQUAL | ALT_NOT_EQUAL |
                                   LESS | LESS_EQUAL |
                                   GREATER | GREATER_EQUAL;
            expression.Rule = aggregateVariableReference | aggregateVariableReferenceExpression |
                              singletonVariableReference | singletonVariableReferenceExpression |
                              numberLiteral |
                              characterLiteral |
                              itemName;

            expanderCountStatement.Rule = functionInvocation | numberLiteral | counterReference;
            scopeLimitStatement.Rule = functionInvocation | numberLiteral | counterReference;
            scopeStatement.Rule = scopeLimitStatement + RANGE + scopeLimitStatement;
            expanderScopeStatement.Rule = scopeStatement | expanderCountStatement;

            multiCounterDeclaration.Rule = MakePlusRule(multiCounterDeclaration, COMMA, counterDeclaration);
            multiExpanderScopeStatement.Rule = MakePlusRule(multiExpanderScopeStatement, COMMA, expanderScopeStatement);
            multiExpanderStatement.Rule = PIPE + multiCounterDeclaration + IN + multiExpanderScopeStatement;

            binaryExpression.Rule = expression + binaryOperators + expression;

            constraintExpression.Rule = binaryExpression |
                                        binaryExpression + multiExpanderStatement |
                                        Empty;

            Root = constraintExpression;

            MarkTransient(binaryOperators, infixOperators, functionName);
            MarkPunctuation(PIPE, RANGE);
            MarkPunctuation(COMMA);
            MarkPunctuation(OPEN_ARG, CLOSE_ARG);

            RegisterBracePair("(", ")");
            MarkReservedWords("in");
        }
    }
}