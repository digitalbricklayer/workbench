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
        private readonly IViewModelService cache;

        public ConstraintMapper(IViewModelService theService)
        {
            if (theService == null)
                throw new ArgumentNullException("theService");
            this.cache = theService;
        }

        internal ConstraintViewModel MapFrom(ConstraintGraphicModel theConstraintModel)
        {
            Debug.Assert(theConstraintModel.HasIdentity);

            var constraintViewModel = new ExpressionConstraintViewModel((ExpressionConstraintGraphicModel) theConstraintModel);

            this.cache.CacheGraphic(constraintViewModel);

            return constraintViewModel;
        }
    }
}
