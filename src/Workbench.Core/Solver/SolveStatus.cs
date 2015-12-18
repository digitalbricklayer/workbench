namespace Workbench.Core.Solver
{
    public enum SolveStatus
    {
        /// <summary>
        /// Failed to solve the model.
        /// </summary>
        Fail,

        /// <summary>
        /// Successfully solved the model.
        /// </summary>
        Success,

        /// <summary>
        /// The model is invalid and cannot be solved.
        /// </summary>
        InvalidModel
    }
}