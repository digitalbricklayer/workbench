using System;

namespace DynaApp.Solver
{
    class SolveResult
    {
        public SolveStatus Status { get; private set; }
        public Solution Solution { get; private set; }

        public SolveResult(SolveStatus theStatus, Solution theSolution)
        {
            if (theSolution == null)
                throw new ArgumentNullException("theSolution");

            this.Status = theStatus;
            this.Solution = theSolution;
        }

        private SolveResult(SolveStatus theStatus)
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
    }
}