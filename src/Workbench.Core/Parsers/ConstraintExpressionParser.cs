using System;
using System.Collections.Generic;
using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public sealed class ConstraintExpressionParser
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

        private static ParseResult<ConstraintExpressionNode> CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ParseResult<ConstraintExpressionNode>(ConvertStatusFrom(parseTree.Status),
                                                                     new List<string>());

                case ParseTreeStatus.Parsed:
                    return new ParseResult<ConstraintExpressionNode>(ParseStatus.Success,
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
