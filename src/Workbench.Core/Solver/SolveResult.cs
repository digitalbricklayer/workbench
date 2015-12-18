using System;
using Workbench.Core.Models;

namespace Workbench.Core.Solver
{
    /// <summary>
    /// The constraint model solution.
    /// </summary>
    public sealed class SolveResult
    {
        /// <summary>
        /// Gets the solution status.
        /// </summary>
        public SolveStatus Status { get; private set; }

        /// <summary>
        /// Gets the model solution.
        /// </summary>
        public SolutionModel Solution { get; private set; }

        /// <summary>
        /// Initialize the soltion result with status and solution.
        /// </summary>
        /// <param name="theStatus">Solve status.</param>
        /// <param name="theSolution">Solution.</param>
        public SolveResult(SolveStatus theStatus, SolutionModel theSolution)
        {
            if (theSolution == null)
                throw new ArgumentNullException("theSolution");

            this.Status = theStatus;
            this.Solution = theSolution;
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
        /// Gets whether the solve succeeded.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.Status == SolveStatus.Success;
            }
        }
    }
}
