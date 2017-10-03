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
        private const string FunctionNamePattern = @"\b[A-Za-z]\w*\b";
        private const string FunctionArgumentStringLiteralPattern = @"\b[A-Za-z]\w*\b";
        private const string VariableRegexPattern = @"\b[A-Za-z]\w*\b";

        public VariableDomainGrammar()
            : base(caseSensitive: false)
        {
            LanguageFlags = LanguageFlags.CreateAst |
                            LanguageFlags.NewLineBeforeEOF;

            var RANGE = ToTerm("..", "range");
            var FUNCTION_CALL_ARGUMENT_SEPERATOR = ToTerm(",");
            var OPEN_ARG = ToTerm("(");
            var CLOSE_ARG = ToTerm(")");

            // Terminals
            var numberLiteral = new NumberLiteral("number literal", NumberOptions.IntOnly, typeof (NumberLiteralNode));
            var functionCallArgumentStringLiteral = new RegexBasedTerminal("function call argument string literal", FunctionArgumentStringLiteralPattern);
            functionCallArgumentStringLiteral.AstConfig.NodeType = typeof (FunctionCallArgumentStringLiteralNode);
            var functionName = new RegexBasedTerminal("function name", FunctionNamePattern);
            functionName.AstConfig.NodeType = typeof (FunctionNameNode);
            var domainName = new RegexBasedTerminal("domain name", VariableRegexPattern);
            domainName.AstConfig.NodeType = typeof(DomainNameNode);

            // Non-terminals
            var domainExpression = new NonTerminal("domainExpression", typeof (DomainExpressionNode));
            var bandExpression = new NonTerminal("expression", typeof (BandExpressionNode));
            var functionCall = new NonTerminal("function call", typeof (FunctionCallXNode));
            var functionCallArgumentList = new NonTerminal("function call arguments", typeof (FunctionArgumentListNode));
            var functionCallArgument = new NonTerminal("function argument", typeof (FunctionCallArgumentNode));
            var sharedDomainReference = new NonTerminal("shared domain reference", typeof(SharedDomainReferenceNode));
            var variableDomainExpression = new NonTerminal("variable domain expression", typeof(VariableDomainExpressionNode));

            // BNF rules
            functionCallArgument.Rule = numberLiteral | functionCallArgumentStringLiteral;
            functionCall.Rule = functionName + OPEN_ARG + functionCallArgumentList + CLOSE_ARG;
            functionCallArgumentList.Rule = MakePlusRule(functionCallArgumentList, FUNCTION_CALL_ARGUMENT_SEPERATOR, functionCallArgument);
            bandExpression.Rule = numberLiteral | functionCall;
            domainExpression.Rule = bandExpression + RANGE + bandExpression;
            sharedDomainReference.Rule = domainName;
            variableDomainExpression.Rule = domainExpression | sharedDomainReference;

            Root = variableDomainExpression;

            MarkPunctuation(RANGE, FUNCTION_CALL_ARGUMENT_SEPERATOR);
            MarkPunctuation(OPEN_ARG, CLOSE_ARG);

            RegisterBracePair("(", ")");
        }
    }
}