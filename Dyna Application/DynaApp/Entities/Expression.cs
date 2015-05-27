using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// An expression can either be a variable or literal.
    /// </summary>
    class Expression
    {
        private Expression(Variable theVariable)
        {
            if (theVariable == null)
                throw new ArgumentNullException("theVariable");
            this.Variable = theVariable;
        }

        private Expression(Literal theLiteral)
        {
            if (theLiteral == null)
                throw new ArgumentNullException("theLiteral");
            this.Literal = theLiteral;
        }

        public Variable Variable { get; set; }
        public Literal Literal { get; set; }

        /// <summary>
        /// Is the expression a variable?
        /// </summary>
        public bool IsVarable
        {
            get
            {
                return this.Variable != null;
            }
        }

        /// <summary>
        /// Is the expression a literal?
        /// </summary>
        public bool IsLiteral
        {
            get
            {
                return this.Literal != null;
            }
        }

        /// <summary>
        /// Create an indentifier expression.
        /// </summary>
        /// <param name="newIdentifier">New identifier.</param>
        /// <returns>Expression.</returns>
        public static Expression CreateIdentifier(string newIdentifier)
        {
            return new Expression(new Variable(newIdentifier));
        }

        /// <summary>
        /// Create a literal expression.
        /// </summary>
        /// <param name="newLiteral">New literal.</param>
        /// <returns>Expression.</returns>
        public static Expression CreateLiteral(string newLiteral)
        {
            return new Expression(new Literal(newLiteral));
        }
    }
}