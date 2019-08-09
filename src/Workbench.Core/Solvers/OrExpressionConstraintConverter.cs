using Workbench.Core.Models;
using Workbench.Core.Repeaters;

namespace Workbench.Core.Solvers
{
    /// <summary>
    /// Convert the expression constraint model representation into a representation usable 
    /// by the or-tools solver.
    /// </summary>
    internal class OrExpressionConstraintConverter
    {
        private readonly OrToolsCache cache;
        private readonly Google.OrTools.ConstraintSolver.Solver solver;
        private readonly ModelModel model;
        private readonly OrValueMapper valueMapper;

        /// <summary>
        /// Initialize the expression constraint converter with a solver and or-tools cache.
        /// </summary>
        /// <param name="theSolver">Google or-tools solver instance.</param>
        /// <param name="theCache">Cache mapping between the model and Google or-tools solver.</param>
        /// <param name="theModel">Model</param>
        internal OrExpressionConstraintConverter(Google.OrTools.ConstraintSolver.Solver theSolver, OrToolsCache theCache, ModelModel theModel, OrValueMapper theValueMapper)
        {
            this.solver = theSolver;
            this.cache = theCache;
            this.model = theModel;
            this.valueMapper = theValueMapper;
        }

        /// <summary>
        /// Map the expression constraint model into the or-tools solver.
        /// </summary>
        /// <param name="constraint">Expression constraint model.</param>
        internal void ProcessConstraint(ExpressionConstraintModel constraint)
        {
            var repeater = new OrConstraintRepeater(this.solver, this.cache, this.model, this.valueMapper);
            repeater.Process(repeater.CreateContextFrom(constraint));
        }
    }
}
