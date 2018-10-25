using System;
using System.Linq;
using Irony.Parsing;

namespace Workbench.Core.Parsers
{
    /// <summary>
    /// Base class for all expression parsers.
    /// </summary>
    /// <typeparam name="T">AST root node.</typeparam>
    public class ExpressionParser<T>
    {
        /// <summary>
        /// Create a parse result from the parse tree created by the Irony parser.
        /// </summary>
        /// <param name="parseTree">Parse tree created by Irony parser.</param>
        /// <returns>Parse result.</returns>
        protected static ParseResult<T> CreateResultFrom(ParseTree parseTree)
        {
            switch (parseTree.Status)
            {
                case ParseTreeStatus.Error:
                    return new ParseResult<T>(ConvertStatusFrom(parseTree.Status), Enumerable.Empty<string>());

                case ParseTreeStatus.Parsed:
                    return new ParseResult<T>(ParseStatus.Success, parseTree);

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Convert the Irony parse status into a Workbench status.
        /// </summary>
        /// <param name="status">Irony parse status.</param>
        /// <returns>Workbench parse status.</returns>
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