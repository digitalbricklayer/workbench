using System;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// A solution to the model.
    /// </summary>
    public sealed class SolveResult
    {
        /// <summary>
        /// Gets the solution status.
        /// </summary>
        public SolveStatus Status { get; }

        /// <summary>
        /// Gets the time taken to solve the model.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets the model solution.
        /// </summary>
        public SolutionSnapshot Snapshot { get; }

        /// <summary>
        /// Initialize the solution result with status and solution.
        /// </summary>
        /// <param name="theStatus">Solve status.</param>
        /// <param name="theSnapshot">A solution snapshot.</param>
        /// <param name="duration">Time taken to solve.</param>
        public SolveResult(SolveStatus theStatus, SolutionSnapshot theSnapshot, TimeSpan duration)
        {
            this.Status = theStatus;
            this.Snapshot = theSnapshot;
            this.Duration = duration;
        }

        /// <summary>
        /// Initialize the solution result with a failure status.
        /// </summary>
        /// <param name="theStatus">Solve status. Must be one of the failure statuses.</param>
        internal SolveResult(SolveStatus theStatus)
        {
            Status = theStatus;
        }

        /// <summary>
        /// Gets a solve result with a failed status.
        /// </summary>
        public static SolveResult Failed
        {
            get
            {
                return new SolveResult(SolveStatus.Fail);
            }
        }

        /// <summary>
        /// Gets a solve result with an invalid model status.
        /// </summary>
        public static SolveResult InvalidModel
        {
            get
            {
                return new SolveResult(SolveStatus.InvalidModel);
            }
        }

        /// <summary>
        /// Gets whether the result resolved the model successfully.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.Status == SolveStatus.Success;
            }
        }

        /// <summary>
        /// Gets whether the result resolved the model successfully.
        /// </summary>
        public bool IsFailure
        {
            get
            {
                return this.Status != SolveStatus.Success;
            }
        }
    }
}
