using System.Diagnostics;
using Dyna.Core.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    /// <summary>
    /// Maps a variable model into a view model.
    /// </summary>
    internal class VariableMapper
    {
        private readonly ModelViewModelCache cache;

        internal VariableMapper(ModelViewModelCache theCache)
        {
            this.cache = theCache;
        }

        internal VariableViewModel MapFrom(VariableModel theVariableModel)
        {
            Debug.Assert(theVariableModel.HasIdentity);

            var variableViewModel = new VariableViewModel
            {
                Model = theVariableModel,
                Name = theVariableModel.Name,
                X = theVariableModel.X,
                Y = theVariableModel.Y
            };

            this.cache.CacheVariable(variableViewModel);

            return variableViewModel;
        }
    }
}