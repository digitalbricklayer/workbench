using System;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An expression can either be a reference to a singleton variable, a 
    /// reference to a single variable in an aggregate or a literal.
    /// </summary>
    [Serializable]
    public class Expression
    {
        private Expression(VariableModel theVariable)
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

        private Expression(AggregateVariableReference theReference)
        {
            if (theReference == null)
                throw new ArgumentNullException("theReference");
            this.AggregateReference = theReference;
        }

        public Expression()
        {
            
        }

        public VariableModel Variable { get; private set; }
        public Literal Literal { get; private set; }

        public AggregateVariableReference AggregateReference { get; private set; }

        /// <summary>
        /// Gets whether the expression involves either a singleton 
        /// variable or an aggregate variable.
        /// </summary>
        public bool IsVarable
        {
            get
            {
                return this.IsSingleton || this.IsAggregate;
            }
        }

        /// <summary>
        /// Gets whether the expression involves a literal?
        /// </summary>
        public bool IsLiteral
        {
            get
            {
                return this.Literal != null;
            }
        }

        /// <summary>
        /// Gets whether the expression involves a singleton variable.
        /// </summary>
        public bool IsSingleton
        {
            get
            {
                return this.Variable != null;
            }
        }

        /// <summary>
        /// Gets whether the expression involves an aggrage variable.
        /// </summary>
        public bool IsAggregate
        {
            get
            {
                return this.AggregateReference != null;
            }
        }

        /// <summary>
        /// Create an indentifier expression.
        /// </summary>
        /// <param name="newIdentifier">New identifier.</param>
        /// <returns>Expression.</returns>
        public static Expression CreateSingletonReference(VariableModel newIdentifier)
        {
            return new Expression(newIdentifier);
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

        /// <summary>
        /// Create an aggregate reference expression.
        /// </summary>
        /// <param name="theReference">Aggregate variable reference.</param>
        /// <returns>Expression</returns>
        public static Expression CreateAggregateReference(AggregateVariableReference theReference)
        {
            return new Expression(theReference);
        }
    }
}