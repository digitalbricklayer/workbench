using System;
using System.Collections.Generic;
using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public class SharedDomainExpressionParser
    {
        private readonly SharedDomainGrammar grammar = new SharedDomainGrammar();

        /// <summary>
        /// Parse a raw domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<SharedDomainExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(grammar);
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }

        private static ParseResult<SharedDomainExpressionNode> CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ParseResult<SharedDomainExpressionNode>(ConvertStatusFrom(parseTree.Status),
                                                                 new List<string>());

                case ParseTreeStatus.Parsed:
                    return new ParseResult<SharedDomainExpressionNode>(ParseStatus.Success, parseTree);

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
