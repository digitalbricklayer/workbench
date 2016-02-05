using System;
using System.Diagnostics;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a constraint model into a view model.
    /// </summary>
    internal class ConstraintMapper
    {
        private readonly ViewModelCache cache;

        internal ConstraintMapper(ViewModelCache theCache)
        {
            if (theCache == null)
                throw new ArgumentNullException("theCache");
            this.cache = theCache;
        }

        internal ConstraintViewModel MapFrom(ConstraintModel theConstraintModel)
        {
            Debug.Assert(theConstraintModel.HasIdentity);

            var constraintViewModel = new ConstraintViewModel(theConstraintModel);

            this.cache.CacheGraphic(constraintViewModel);

            return constraintViewModel;
        }
    }
}