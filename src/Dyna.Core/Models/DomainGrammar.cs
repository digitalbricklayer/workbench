using System;
using System.Linq;
using Sprache;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Grammar for parsing a domain expression.
    /// </summary>
    internal class DomainGrammar
    {
        /// <summary>
        /// Parse a band.
        /// </summary>
        private static readonly Parser<string> bandGrammar =
            from band in Sprache.Parse.Number.Token()
            select band;

        /// <summary>
        /// Parse the range specifier.
        /// </summary>
        private static readonly Parser<string> rangeSpecifierGrammar =
            from leading in Sprache.Parse.WhiteSpace.Many()
            from x in Sprache.Parse.String("..").Token()
            from trailing in Sprache.Parse.WhiteSpace.Many()
            select new String(x.ToArray());

        private static readonly Parser<DomainExpressionModel> rangeExpressionGrammar =
            from lowerBand in bandGrammar
            from rangeSpecifier in rangeSpecifierGrammar
            from upperBand in bandGrammar
            select new DomainExpressionModel(Convert.ToInt32(upperBand), Convert.ToInt32(lowerBand));

        public static DomainExpressionModel Parse(string rawExpression)
        {
            return rangeExpressionGrammar.End().Parse(rawExpression);
        }
    }
}