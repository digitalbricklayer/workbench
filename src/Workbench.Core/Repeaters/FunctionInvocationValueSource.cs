using System;
using System.Diagnostics;
using System.Linq;

namespace Workbench.Core.Repeaters
{
    /// <summary>
    /// Implementation of the limit value source for a limit tied to the 
    /// invocation of a function.
    /// </summary>
    internal sealed class FunctionInvocationValueSource : ILimitValueSource
    {
        private readonly FunctionInvocationContext context;

        /// <summary>
        /// Initialize the function invocation limit value with a counter context.
        /// </summary>
        /// <param name="theContext">Counter context.</param>
        public FunctionInvocationValueSource(FunctionInvocationContext theContext)
        {
            this.context = theContext;
        }

        /// <summary>
        /// Get the current value of the scope limit.
        /// </summary>
        /// <returns>Current scope limit value.</returns>
        public int GetValue()
        {
            Debug.Assert(this.context != null);
            Debug.Assert(this.context.FunctionInvocation.FunctionName == "size");
            var variableName = this.context.FunctionInvocation.ArgumentList.Arguments.First();
            var variable = this.context.Model.GetVariableByName(variableName.Value.Value);
            if (variable != null)
                return Convert.ToInt32(variable.GetSize());
            var bucket = this.context.Model.GetBucketByName(variableName.Value.Value);
            return Convert.ToInt32(bucket.GetSize());
        }
    }
}