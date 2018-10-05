using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public class SharedDomainExpressionParser : ExpressionParser<SharedDomainExpressionNode>
    {
        /// <summary>
        /// Parse a raw domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<SharedDomainExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(new SharedDomainGrammar());
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }
    }
}
