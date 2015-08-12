using System;
using System.Collections.Generic;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class ModelMapper
    {
        private WorkspaceMapper workspaceMapper;
        private readonly VariableMapper variableMapper;
        private readonly DomainMapper domainMapper;
        private readonly ConstraintMapper constraintMapper;
        private readonly Dictionary<string, GraphicViewModel> graphicMap;

        internal ModelMapper(WorkspaceMapper theWorkspaceMapper)
        {
            this.workspaceMapper = theWorkspaceMapper;
            this.variableMapper = new VariableMapper();
            this.domainMapper = new DomainMapper();
            this.constraintMapper = new ConstraintMapper();
            this.graphicMap = new Dictionary<string, GraphicViewModel>();
        }

        internal ModelViewModel MapFrom(ModelModel theModelModel)
        {
            var modelViewModel = new ModelViewModel();

            foreach (var domainModel in theModelModel.Domains)
            {
                var domainViewModel = this.domainMapper.MapFrom(domainModel);
                modelViewModel.AddDomain(domainViewModel);
                this.graphicMap.Add(domainViewModel.Name, domainViewModel);
            }

            foreach (var constraintModel in theModelModel.Constraints)
            {
                var constraintViewModel = this.constraintMapper.MapFrom(constraintModel);
                modelViewModel.AddConstraint(constraintViewModel);
                this.graphicMap.Add(constraintViewModel.Name, constraintViewModel);
            }

            foreach (var variableModel in theModelModel.Variables)
            {
                var variableViewModel = this.variableMapper.MapFrom(variableModel);
                modelViewModel.AddVariable(variableViewModel);
            }

            foreach (var connectionModel in theModelModel.Connections)
            {
                var variableViewModel = this.GetVariableByName(connectionModel.DestinationConnector.Parent.Name);
                var connectedTo = this.GetByName(connectionModel.SourceConnector.Parent.Name);
                modelViewModel.Connect(variableViewModel, connectedTo);
            }

            return modelViewModel;
        }

        private GraphicViewModel GetByName(string theName)
        {
            if (string.IsNullOrWhiteSpace(theName))
                throw new ArgumentException("theName");
            return this.graphicMap[theName];
        }

        private VariableViewModel GetVariableByName(string theVariableName)
        {
            if (string.IsNullOrWhiteSpace(theVariableName))
                throw new ArgumentException("theVariableName");
            return this.variableMapper.GetVariableByName(theVariableName);
        }
    }
}