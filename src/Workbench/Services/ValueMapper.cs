using System;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Map a value model into a view model.
    /// </summary>
    internal class ValueMapper
    {
        private readonly IViewModelCache cache;

        internal ValueMapper(IViewModelCache theCache)
        {
            if (theCache == null)
                throw new ArgumentNullException("theCache");
            this.cache = theCache;
        }

        /// <summary>
        /// Map a value model into a view model.
        /// </summary>
        /// <param name="theValueModel">Value model.</param>
        /// <returns>Value view model.</returns>
        internal ValueViewModel MapFrom(ValueModel theValueModel)
        {
            var variableViewModel = this.cache.GetVariableByIdentity(theValueModel.Variable.Id);
            return new ValueViewModel(variableViewModel)
            {
                Model = theValueModel
            };
        }
    }
}