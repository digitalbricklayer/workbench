using System;
using System.Diagnostics;

namespace Workbench.Services
{
#if true
    /// <summary>
    /// Maps a constraint model into a view model.
    /// </summary>
    public sealed class ConstraintMapper
    {
        private readonly IViewModelService cache;

        public ConstraintMapper(IViewModelService theService)
        {
            if (theService == null)
                throw new ArgumentNullException(nameof(theService));
            this.cache = theService;
        }

        public ConstraintVisualizerViewModel MapFrom(ConstraintGraphicModel theConstraintModel)
        {
            Debug.Assert(theConstraintModel.HasIdentity);

            var constraintViewModel = new ExpressionConstraintViewModel((ExpressionConstraintGraphicModel) theConstraintModel);

            this.cache.CacheGraphic(constraintViewModel);

            return constraintViewModel;
            return null;
        }

    }
#endif
}
