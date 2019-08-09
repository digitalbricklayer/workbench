using System;
using System.Diagnostics;
using Workbench.Core.Models;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Convert the model representation into a representation usable by the or-tools solver.
    /// </summary>
    internal class ModelConverter
    {
        private readonly OrConstraintConverter constraintConverter;
        private readonly VariableConverter variableConverter;
        private readonly OrBucketConverter bucketConverter;

        /// <summary>
        /// Initialize the model converter with a Google or-tools solver.
        /// </summary>
        internal ModelConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, OrValueMapper valueMapper)
        {
            this.constraintConverter = new OrConstraintConverter(theSolver, theCache, valueMapper);
            this.variableConverter = new VariableConverter(theSolver, theCache, valueMapper);
            this.bucketConverter = new OrBucketConverter(theSolver, theCache, valueMapper);
        }

        /// <summary>
        /// Convert the model into a representation used by the Google or-tools solver.
        /// </summary>
        /// <param name="theModel">The model model.</param>
        internal void ConvertFrom(ModelModel theModel)
        {
            Debug.Assert(this.constraintConverter != null);
            Debug.Assert(this.variableConverter != null);

            this.variableConverter.ConvertVariables(theModel);
            this.bucketConverter.ConvertBuckets(theModel);
            this.constraintConverter.ProcessConstraints(theModel);
        }
    }
}
