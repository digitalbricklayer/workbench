using System.Diagnostics;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class DomainMapper
    {
        private readonly ConnectorMapper connectorMapper;
        private readonly ModelViewModelCache cache;

        internal DomainMapper(ModelViewModelCache theCache)
        {
            this.cache = theCache;
            this.connectorMapper = new ConnectorMapper(this.cache);
        }

        internal DomainViewModel MapFrom(DomainModel theDomainModel)
        {
            Debug.Assert(theDomainModel.HasIdentity);

            var domainViewModel = new DomainViewModel();
            domainViewModel.Model = theDomainModel;
            domainViewModel.Id = theDomainModel.Id;
            domainViewModel.Name = theDomainModel.Name;
            domainViewModel.Expression.Text = theDomainModel.Expression.Text;
            domainViewModel.X = theDomainModel.X;
            domainViewModel.Y = theDomainModel.Y;
            foreach (var connectorModel in theDomainModel.Connectors)
            {
                var connectorViewModel = this.connectorMapper.MapFrom(connectorModel);
                domainViewModel.AddConnector(connectorViewModel);
            }

            this.cache.CacheGraphic(domainViewModel);

            return domainViewModel;
        }
    }
}