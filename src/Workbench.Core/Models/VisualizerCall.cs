using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Workbench.Core.Models
{
    [Serializable]
    public class VisualizerCall
    {
        private readonly IList<CallArgument> arguments;

        public VisualizerCall(IEnumerable<CallArgument> theArguments)
        {
            Contract.Requires<ArgumentNullException>(theArguments != null);
            this.arguments = new List<CallArgument>(theArguments);
        }

        public IReadOnlyCollection<CallArgument> Arguments
        {
            get { return new ReadOnlyCollection<CallArgument>(this.arguments); }
        }

        public string GetArgumentByName(string theName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(theName));
            var x = this.arguments.FirstOrDefault(argument => argument.Name == theName);
            Contract.Assert(x != null);
            return x.Value;
        }
    }
}
