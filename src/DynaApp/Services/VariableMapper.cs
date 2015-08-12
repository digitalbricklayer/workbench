using System;
using System.Collections.Generic;
using DynaApp.Models;
using DynaApp.ViewModels;

namespace DynaApp.Services
{
    internal class VariableMapper
    {
        private readonly Dictionary<string, VariableViewModel> variableMap;

        public VariableMapper()
        {
            this.variableMap = new Dictionary<string, VariableViewModel>();
        }

        internal VariableViewModel MapFrom(VariableModel theVariableModel)
        {
            var variableViewModel = new VariableViewModel
            {
                Name = theVariableModel.Name,
                X = theVariableModel.X,
                Y = theVariableModel.Y
            };

            this.variableMap.Add(variableViewModel.Name, variableViewModel);

            return variableViewModel;
        }

        internal VariableViewModel GetVariableByName(string theVariableName)
        {
            if (string.IsNullOrWhiteSpace(theVariableName))
                throw new ArgumentException("theVariableName");
            return this.variableMap[theVariableName];
        }
    }
}