using DynaApp.Entities;
using Google.OrTools.ConstraintSolver;

namespace DynaApp.Solver
{
    /// <summary>
    /// Constraint solver.
    /// </summary>
    class ConstraintSolver
    {
        /// <summary>
        /// Solve the problem in the workspace.
        /// </summary>
        /// <param name="theModel">The problem workspace.</param>
        public SolveResult Solve(Model theModel)
        {
            if (!theModel.Validate()) return SolveResult.InvalidModel;
            var solver = new Google.OrTools.ConstraintSolver.Solver(theModel.Name);
            var variables = new IntervalVarVector();
            foreach (var variable in theModel.Variables)
            {
#if false
                var x = new CpIntVector();
                variables.Add(solver.MakeIntVar(x));
                var db = solver.MakePhase(variables, Google.OrTools.ConstraintSolver.Solver.CHOOSE_FIRST_UNBOUND);
                solver.Solve(null, null);
#endif
            }

            return SolveResult.Failed;
        }
    }
}
