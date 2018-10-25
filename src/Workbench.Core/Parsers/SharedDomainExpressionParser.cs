using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    /// <summary>
    /// Parser for the shared domain expression language.
    /// </summary>
    public sealed class SharedDomainExpressionParser : ExpressionParser<SharedDomainExpressionNode>
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
            // 1. first pass: use the Irony grammar
            var parseTree = parser.Parse(rawExpression);
#if false
            if (!parseTree.HasErrors())
            {
                // 2. second pass: find non grammatical errors
                var sharedDomainExpressionNode = (SharedDomainExpressionNode) parseTree.Root.AstNode;
                var variableReferenceCaptureVisitor = new ConstraintVariableReferenceCaptureVisitor();
                sharedDomainExpressionNode.AcceptVisitor(variableReferenceCaptureVisitor);
            }
#endif

            return CreateResultFrom(parseTree);
        }
    }
}
