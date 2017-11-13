using Irony.Parsing;
using Workbench.Core.Nodes;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for variable inline domain expressions.
    /// </summary>
    [Language("Variable Domain Expression Grammar", "0.1", "A grammar for variable inline domain expressions.")]
    internal class VariableDomainGrammar : Grammar
    {
        public VariableDomainGrammar()
            : base(caseSensitive: false)
        {
            LanguageFlags = LanguageFlags.CreateAst |
                            LanguageFlags.NewLineBeforeEOF;

            var RANGE = ToTerm("..", "range");
            var COMMA = ToTerm(",");
            var OPEN_ARG = ToTerm("(");
            var CLOSE_ARG = ToTerm(")");

            // Terminals
            var numberLiteral = new NumberLiteral("number literal", NumberOptions.IntOnly, typeof (NumberLiteralNode));
            var itemName = new IdentifierTerminal("string literal", IdOptions.IsNotKeyword);
            itemName.AstConfig.NodeType = typeof(ItemNameNode);
            var functionCallArgumentStringLiteral = new IdentifierTerminal("function call argument string literal");
            functionCallArgumentStringLiteral.AstConfig.NodeType = typeof (FunctionCallArgumentStringLiteralNode);
            var functionName = new IdentifierTerminal("function name");
            functionName.AstConfig.NodeType = typeof (FunctionNameNode);
            var domainName = new IdentifierTerminal("domain name");
            domainName.AddPrefix("$", IdOptions.NameIncludesPrefix);
            domainName.AstConfig.NodeType = typeof(DomainNameNode);

            // Non-terminals
            var domainExpression = new NonTerminal("domainExpression", typeof (VariableDomainExpressionNode));
            var rangeDomainExpression = new NonTerminal("range domain expression", typeof(RangeDomainExpressionNode));
            var itemsList = new NonTerminal("list items", typeof(ItemsListNode));
            var listDomainExpression = new NonTerminal("list domain expression", typeof(ListDomainExpressionNode));
            var bandExpression = new NonTerminal("expression", typeof (BandExpressionNode));
            var functionCall = new NonTerminal("function call", typeof (FunctionInvocationNode));
            var functionCallArgumentList = new NonTerminal("function call arguments", typeof (FunctionArgumentListNode));
            var functionCallArgument = new NonTerminal("function argument", typeof (FunctionCallArgumentNode));
            var sharedDomainReference = new NonTerminal("shared domain reference", typeof(SharedDomainReferenceNode));

            // BNF rules
            itemsList.Rule = MakePlusRule(itemsList, COMMA, itemName);
            listDomainExpression.Rule = itemsList;
            functionCallArgument.Rule = numberLiteral | functionCallArgumentStringLiteral;
            functionCall.Rule = functionName + OPEN_ARG + functionCallArgumentList + CLOSE_ARG;
            functionCallArgumentList.Rule = MakePlusRule(functionCallArgumentList, COMMA, functionCallArgument);
            bandExpression.Rule = numberLiteral | functionCall;
            rangeDomainExpression.Rule = bandExpression + RANGE + bandExpression;
            sharedDomainReference.Rule = domainName;
            domainExpression.Rule = rangeDomainExpression | sharedDomainReference| listDomainExpression;

            Root = domainExpression;

            MarkPunctuation(RANGE, COMMA);
            MarkPunctuation(OPEN_ARG, CLOSE_ARG);

            RegisterBracePair("(", ")");
        }
    }
}