using System.Diagnostics;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ConstraintMapper
    {
        private readonly ConnectorMapper connectorMapper;
        private readonly ModelViewModelCache cache;

        internal ConstraintMapper(ModelViewModelCache theCache)
        {
            this.cache = theCache;
            this.connectorMapper = new ConnectorMapper(theCache);
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
            foreach (var connectorModel in theConstraintModel.Connectors)
            {
                var connectorViewModel = this.connectorMapper.MapFrom(connectorModel);
                constraintViewModel.AddConnector(connectorViewModel);
            }

            this.cache.CacheGraphic(constraintViewModel);

            return constraintViewModel;
        }
    }
}