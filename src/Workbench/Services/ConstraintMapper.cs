using System;
using System.Diagnostics;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a constraint model into a view model.
    /// </summary>
    public class ConstraintMapper
    {
        private readonly IViewModelCache cache;

        public ConstraintMapper(IViewModelCache theCache)
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