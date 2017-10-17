using Workbench.Core.Models;
using Workbench.Core.Nodes;

namespace Workbench.Core.Repeaters
{
    internal class FunctionInvocationContext
    {
        public FunctionInvocationContext(FunctionInvocationNode theFunctionInvocationNode, ModelModel theModel)
        {
            FunctionInvocation = theFunctionInvocationNode;
            Model = theModel;
        }

        public FunctionInvocationNode FunctionInvocation { get; private set; }
        public ModelModel Model { get; private set; }
    }
}