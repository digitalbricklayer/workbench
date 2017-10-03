using System;
using System.Collections.Generic;
using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Nodes;

namespace Workbench.Core.Parsers
{
    public class VariableDomainExpressionParser
    {
        private readonly VariableDomainGrammar grammar = new VariableDomainGrammar();

        /// <summary>
        /// Parse a raw variable domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw variable domain expression.</param>
        /// <returns>Parse result.</returns>
        public ParseResult<VariableDomainExpressionNode> Parse(string rawExpression)
        {
            var language = new LanguageData(grammar);
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }

        private static ParseResult<VariableDomainExpressionNode> CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ParseResult<VariableDomainExpressionNode>(ConvertStatusFrom(parseTree.Status),
                                                                 new List<string>());

                case ParseTreeStatus.Parsed:
                    return new ParseResult<VariableDomainExpressionNode>(ParseStatus.Success, parseTree);

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
