namespace Workbench.Core.Repeaters
{
    internal interface ICounterIterator
    {
        /// <summary>
        /// Gets the counter's current value.
        /// </summary>
        int CurrentValue { get; }

        /// <summary>
        /// Advance the counter to the next value of the range.
        /// </summary>
        /// <returns>True if another value is available. False if the end of 
        /// the range has been reached.</returns>
        bool Next();

        /// <summary>
        /// Reset the current value to its initial position.
        /// </summary>
        void Reset();
    }
}