using System;
using System.Linq;
using Irony.Parsing;

namespace Workbench.Core.Parsers
{
    public class ExpressionParser<T>
    {
        protected static ParseResult<T> CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ParseResult<T>(ConvertStatusFrom(parseTree.Status), Enumerable.Empty<string>());

                case ParseTreeStatus.Parsed:
                    return new ParseResult<T>(ParseStatus.Success,
                        parseTree);

                default:
                    throw new NotImplementedException();
            }
        }

        protected static ParseStatus ConvertStatusFrom(ParseTreeStatus status)
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