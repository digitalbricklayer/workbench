using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// A solution to the model.
    /// </summary>
    public sealed class SolveResult
    {
        /// <summary>
        /// Gets the solution status.
        /// </summary>
        public SolveStatus Status { get; private set; }

        /// <summary>
        /// Gets the time taken to solve the model.
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets the model solution.
        /// </summary>
        public SolutionSnapshot Snapshot { get; private set; }

        /// <summary>
        /// Initialize the soltion result with status and solution.
        /// </summary>
        /// <param name="theStatus">Solve status.</param>
        /// <param name="theDuration">Duration taken to solve the model.</param>
        /// <param name="theSnapshot">A solution snapshot.</param>
        public SolveResult(SolveStatus theStatus, TimeSpan theDuration, SolutionSnapshot theSnapshot)
        {
            Contract.Requires<ArgumentNullException>(theSnapshot != null);

            this.Status = theStatus;
            this.Duration = theDuration;
            this.Snapshot = theSnapshot;
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
