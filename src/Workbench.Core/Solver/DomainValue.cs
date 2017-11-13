namespace Workbench.Core.Solver
{
    /// <summary>
    /// Base class for all domain values.
    /// </summary>
    public abstract class DomainValue
    {
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
    }
}