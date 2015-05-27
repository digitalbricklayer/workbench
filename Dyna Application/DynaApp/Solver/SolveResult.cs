namespace DynaApp.Solver
{
    class SolveResult
    {
        public SolveStatus Status { get; private set; }

        public static SolveResult Failed
        {
            get
            {
                return new SolveResult(SolveStatus.Fail);
            }
        }

        public SolveResult(SolveStatus theStatus)
        {
            Status = theStatus;
        }
    }
}