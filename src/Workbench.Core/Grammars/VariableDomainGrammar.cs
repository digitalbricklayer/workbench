using Sprache;
using Workbench.Core.Expressions;
using Workbench.Core.Models;

namespace Workbench.Core.Grammars
{
    /// <summary>
    /// Grammar for parsing a variable domain expression.
    /// </summary>
    internal class VariableDomainGrammar
    {
        /// <summary>
        /// Parse an identifier.
        /// </summary>
        private static readonly Parser<string> Identifier =
            from first in Sprache.Parse.Letter.Once().Text()
            from rest in Sprache.Parse.LetterOrDigit.Many().Text()
            select string.Concat(first, rest);

        /// <summary>
        /// Empty expression parser.
        /// </summary>
        private static readonly Parser<VariableDomainExpressionUnit> EmptyExpression =
            from leading in Sprache.Parse.WhiteSpace.Many()
            select new VariableDomainExpressionUnit();

        /// <summary>
        /// Shared domain name parser.
        /// </summary>
        private static readonly Parser<VariableDomainExpressionUnit> SharedDomainName =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from sharedDomainName in Identifier
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select new VariableDomainExpressionUnit(new SharedDomainReference(sharedDomainName));

        /// <summary>
        /// Inline domain expression parser.
        /// </summary>
        private static readonly Parser<VariableDomainExpressionUnit> InlineDomainExpression =
            from expression in DomainGrammar.RangeExpressionGrammar
            select new VariableDomainExpressionUnit(new DomainExpressionModel(expression));

        /// <summary>
        /// Parse a raw variable domain expression.
        /// </summary>
        /// <param name="rawExpression">Raw domain expression.</param>
        /// <returns>Parsed expression tree.</returns>
        public static VariableDomainExpressionUnit Parse(string rawExpression)
        {
            var variableDomainExpressionGrammar =
                from expression in SharedDomainName
                    .Or(InlineDomainExpression)
					.Or(EmptyExpression)
				select expression;

            return variableDomainExpressionGrammar.End().Parse(rawExpression);
        }
    }
}
