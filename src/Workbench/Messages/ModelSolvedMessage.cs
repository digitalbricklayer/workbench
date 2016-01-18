using System;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Messages
{
    /// <summary>
    /// Message sent when the model has been solved.
    /// </summary>
    public class ModelSolvedMessage
    {
        /// <summary>
        /// Initialize a model solved message with the solve attempt result.
        /// </summary>
        /// <param name="theResult">Solve attempt result.</param>
        public ModelSolvedMessage(SolveResult theResult)
        {
            if (theResult == null)
                throw new ArgumentNullException("theResult");

            this.Result = theResult;
            this.Solution = theResult.Solution;
        }

        /// <summary>
        /// Gets the solve result.
        /// </summary>
        public SolveResult Result { get; private set; }

        /// <summary>
        /// Gets the model solution.
        /// </summary>
        public SolutionModel Solution { get; private set; }
    }
}