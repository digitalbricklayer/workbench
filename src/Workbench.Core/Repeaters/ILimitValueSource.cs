namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Contract for getting a literal or counter value.
    /// </summary>
    internal interface ILimitValueSource
    {
        /// <summary>
        /// Get the current value of the limit.
        /// </summary>
        /// <returns>Current limit value.</returns>
        int GetValue();
    }
}
