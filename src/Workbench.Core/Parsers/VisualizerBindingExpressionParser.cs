using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    /// <summary>
    /// Parser for the visualizer binding expression.
    /// </summary>
    public sealed class VisualizerBindingExpressionParser : ExpressionParser<VisualizerBindingExpressionNode>
    {
        /// <summary>
        /// Parse a raw visualizer binding expression.
        /// </summary>
        /// <param name="rawExpression">Raw visualizer binding expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<VisualizerBindingExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(new VisualizerBindingGrammar());
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }
    }
}
