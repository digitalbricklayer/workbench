using System;
using System.Collections.Generic;
using Irony.Parsing;
using Workbench.Core.Grammars;
using Workbench.Core.Parsers;

namespace Workbench.Core
{
    public class ConstraintExpressionParser
    {
        private readonly ConstraintGrammar grammar = new ConstraintGrammar();

        /// <summary>
        /// Parse a raw constraint expression.
        /// </summary>
        /// <param name="rawExpression">Raw constraint expression.</param>
        /// <returns>Root expression node.</returns>
        public ConstraintExpressionParseResult Parse(string rawExpression)
        {
            var language = new LanguageData(grammar);
            var parser = new Parser(language);
            var parseTree = parser.Parse(rawExpression);

            return CreateResultFrom(parseTree);
        }

        private static ConstraintExpressionParseResult CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ConstraintExpressionParseResult(ConvertStatusFrom(parseTree.Status),
                                                               new List<string>());

                case ParseTreeStatus.Parsed:
                    return new ConstraintExpressionParseResult(ConstraintExpressionParseStatus.Success,
                                                               parseTree);

                default:
                    throw new NotImplementedException();
            }
        }

        private static ConstraintExpressionParseStatus ConvertStatusFrom(ParseTreeStatus status)
        {
            switch (status)
            {
                case ParseTreeStatus.Parsed:
                    return ConstraintExpressionParseStatus.Success;

                case ParseTreeStatus.Error:
                    return ConstraintExpressionParseStatus.Failed;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
