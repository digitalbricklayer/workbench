using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    /// <summary>
    /// Parser for the visualizer binding expression.
    /// </summary>
    public sealed class VisualizerBindingExpressionParser
    {
        private readonly VisualizerBindingGrammar grammar = new VisualizerBindingGrammar();

        /// <summary>
        /// Parse a raw visualizer binding expression.
        /// </summary>
        /// <param name="rawExpression">Raw visualizer binding expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<VisualizerExpressionNode> Parse(string rawExpression)
        {
            Contract.Requires<ArgumentNullException>(rawExpression != null);
            Contract.Ensures(Contract.Result<ParseResult<VisualizerExpressionNode>>() != null);
            var language = new LanguageData(grammar);
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }

        private static ParseResult<VisualizerExpressionNode> CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ParseResult<VisualizerExpressionNode>(ConvertStatusFrom(parseTree.Status),
                                                                                         new List<string>());

                case ParseTreeStatus.Parsed:
                    return new ParseResult<VisualizerExpressionNode>(ParseStatus.Success,
                                                                                         parseTree);

                default:
                    throw new NotImplementedException();
            }
        }

        private static ParseStatus ConvertStatusFrom(ParseTreeStatus status)
        {
            switch (status)
            {
                case ParseTreeStatus.Parsed:
                    return ParseStatus.Success;

                case ParseTreeStatus.Error:
                    return ParseStatus.Failed;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
