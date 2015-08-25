using System.Diagnostics;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ConstraintMapper
    {
        private readonly ModelViewModelCache cache;

        internal ConstraintMapper(ModelViewModelCache theCache)
        {
            this.cache = theCache;
        }

        internal ConstraintViewModel MapFrom(ConstraintModel theConstraintModel)
        {
            Debug.Assert(theConstraintModel.HasIdentity);

            var constraintViewModel = new ConstraintViewModel();
            constraintViewModel.Model = theConstraintModel;
            constraintViewModel.Name = theConstraintModel.Name;
            constraintViewModel.Expression.Text = theConstraintModel.Expression.Text;
            constraintViewModel.X = theConstraintModel.X;
            constraintViewModel.Y = theConstraintModel.Y;

            this.cache.CacheGraphic(constraintViewModel);

            return constraintViewModel;
        }
    }
}