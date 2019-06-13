namespace Workbench.Core.Solvers
{
    internal sealed class EncapsulatedSelector
    {
        internal EncapsulatedSelector(int index)
        {
            Index = index;
        }

        internal int Index { get; }
    }
}
