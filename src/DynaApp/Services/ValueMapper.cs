using System;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Map a value into a view model.
    /// </summary>
    internal class ValueMapper
    {
        private readonly ModelViewModelCache cache;

        internal ValueMapper(ModelViewModelCache theCache)
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