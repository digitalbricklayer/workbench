using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public class VariableDomainExpressionParser : ExpressionParser<VariableDomainExpressionNode>
    {
        /// <summary>
        /// Parse a raw variable domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw variable domain expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<VariableDomainExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(new VariableDomainGrammar());
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }
    }
}
