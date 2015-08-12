using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ConstraintMapper
    {
        internal ConstraintViewModel MapFrom(ConstraintModel theConstraintModel)
        {
            var constraintViewModel = new ConstraintViewModel();
            constraintViewModel.Model = theConstraintModel;
            constraintViewModel.Name = theConstraintModel.Name;
            constraintViewModel.Expression.Text = theConstraintModel.Expression.Text;
            constraintViewModel.X = theConstraintModel.X;
            constraintViewModel.Y = theConstraintModel.Y;

            return constraintViewModel;
        }
    }
}