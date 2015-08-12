using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Map a value into a view model.
    /// </summary>
    internal class ValueMapper
    {
        /// <summary>
        /// Map a value model into a view model.
        /// </summary>
        /// <param name="theValueModel">Value model.</param>
        /// <returns>Value view model.</returns>
        internal ValueViewModel MapFrom(ValueModel theValueModel)
        {
            return new ValueViewModel(theValueModel.Variable.Name)
            {
                Model = theValueModel
            };
        }
    }
}