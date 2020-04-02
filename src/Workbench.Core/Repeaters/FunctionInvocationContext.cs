using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal sealed class FunctionInvocationContext
    {
        public FunctionInvocationContext(FunctionInvocationNode theFunctionInvocationNode, BundleModel bundle)
        {
            FunctionInvocation = theFunctionInvocationNode;
            Bundle = bundle;
        }

        public FunctionInvocationNode FunctionInvocation { get; private set; }
        public BundleModel Bundle { get; private set; }
    }
}