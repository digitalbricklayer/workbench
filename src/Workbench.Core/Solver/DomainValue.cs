namespace Workbench.Core.Solver
{
    /// <summary>
    /// Base class for all domain values.
    /// </summary>
    public abstract class DomainValue
    {
        /// <summary>
        /// Initialize the domain value with a AST node representing the domain expression.
        /// </summary>
        protected DomainValue()
        {
        }

        /// <summary>
        /// Get the domain range.
        /// </summary>
        /// <returns>Domain range.</returns>
        public abstract Range GetRange();

        /// <summary>
        /// Does the domain value intersect with this range.
        /// </summary>
        /// <param name="theDomainValue">Domain value.</param>
        /// <returns>True if intersects. False if it does not intersect.</returns>
        public abstract bool IntersectsWith(DomainValue theDomainValue);

        /// <summary>
        /// Map from the solver value to the model value.
        /// </summary>
        /// <param name="solverValue">Solver value.</param>
        /// <returns>Model value.</returns>
        internal abstract object MapFrom(long solverValue);

        /// <summary>
        /// Map from the model value to the solver value.
        /// </summary>
        /// <param name="modelValue">Model value.</param>
        /// <returns>Solver value.</returns>
        internal abstract int MapTo(object modelValue);
    }
}