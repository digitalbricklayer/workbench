using System.Diagnostics;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class DomainMapper
    {
        private readonly ModelViewModelCache cache;

        internal DomainMapper(ModelViewModelCache theCache)
        {
            this.cache = theCache;
        }

        internal DomainViewModel MapFrom(DomainModel theDomainModel)
        {
            Debug.Assert(theDomainModel.HasIdentity);

            var domainViewModel = new DomainViewModel();
            domainViewModel.Model = theDomainModel;
            domainViewModel.Name = theDomainModel.Name;
            domainViewModel.Expression.Text = theDomainModel.Expression.Text;
            domainViewModel.X = theDomainModel.X;
            domainViewModel.Y = theDomainModel.Y;

            this.cache.CacheGraphic(domainViewModel);

            return domainViewModel;
        }
    }
}