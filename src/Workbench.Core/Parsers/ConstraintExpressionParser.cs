using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public sealed class ConstraintExpressionParser : ExpressionParser<ConstraintExpressionNode>
    {
        private readonly ConstraintExpressionGrammar grammar = new ConstraintExpressionGrammar();

        /// <summary>
        /// Parse a raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<ConstraintExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(grammar);
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }
    }
}
