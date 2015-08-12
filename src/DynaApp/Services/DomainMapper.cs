using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class DomainMapper
    {
        internal DomainViewModel MapFrom(DomainModel theDomainModel)
        {
            var domainViewModel = new DomainViewModel();
            domainViewModel.Name = theDomainModel.Name;
            domainViewModel.Expression.Text = theDomainModel.Expression.Text;
            domainViewModel.X = theDomainModel.X;
            domainViewModel.Y = theDomainModel.Y;

            return domainViewModel;
        }
    }
}