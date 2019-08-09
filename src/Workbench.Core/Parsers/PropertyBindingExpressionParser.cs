using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    /// <summary>
    /// Parser for the visualizer property update expression.
    /// </summary>
    public sealed class PropertyBindingExpressionParser : ExpressionParser<PropertyUpdateExpressionNode>
    {
        /// <summary>
        /// Parse a raw property binding expression.
        /// </summary>
        /// <param name="rawExpression">Raw visualizer binding expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<PropertyUpdateExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(new PropertyBindingGrammar());
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }
    }
}
