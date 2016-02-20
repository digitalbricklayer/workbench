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
        private readonly IViewModelService cache;

        internal ValueMapper(IViewModelService theService)
        {
            if (theService == null)
                throw new ArgumentNullException("theService");
            this.cache = theService;
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