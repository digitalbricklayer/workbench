using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal sealed class FunctionInvocationContext
    {
        public FunctionInvocationContext(FunctionInvocationNode theFunctionInvocationNode, BundleTemplateModel bundle)
        {
            FunctionInvocation = theFunctionInvocationNode;
            Bundle = bundle;
        }

        public FunctionInvocationNode FunctionInvocation { get; private set; }
        public BundleTemplateModel Bundle { get; private set; }
    }
}