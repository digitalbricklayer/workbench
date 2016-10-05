using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(theResult != null);

            Result = theResult;
            Snapshot = theResult.Snapshot;
        }

        /// <summary>
        /// Gets the solve result.
        /// </summary>
        public SolveResult Result { get; private set; }

        /// <summary>
        /// Gets the model solution snapshot.
        /// </summary>
        public SolutionSnapshot Snapshot { get; private set; }
    }
}