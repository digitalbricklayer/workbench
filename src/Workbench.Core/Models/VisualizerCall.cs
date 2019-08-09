using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Workbench.Core.Models
{
    [Serializable]
    public class VisualizerCall
    {
        private readonly IList<CallArgument> arguments;

        public VisualizerCall(IEnumerable<CallArgument> theArguments)
        {
            this.arguments = new List<CallArgument>(theArguments);
        }

        public IReadOnlyCollection<CallArgument> Arguments
        {
            get { return new ReadOnlyCollection<CallArgument>(this.arguments); }
        }

        public string GetArgumentByName(string theName)
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("The name must not be empty", nameof(theName));

            var x = this.arguments.FirstOrDefault(argument => argument.Name == theName);
            Debug.Assert(x != null);
            return x.Value;
        }
    }
}
