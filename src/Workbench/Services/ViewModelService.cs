﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Service responsible for cross cutting concerns across all view models.
    /// </summary>
    public class ViewModelService : IViewModelService
    {
        private readonly Dictionary<int, GraphicViewModel> graphicMap;
        private readonly Dictionary<int, VariableViewModel> variableMap;

        /// <summary>
        /// Initialize the view model cache with default values.
        /// </summary>
        public ViewModelService()
        {
            this.graphicMap = new Dictionary<int, GraphicViewModel>();
            this.variableMap = new Dictionary<int, VariableViewModel>();
        }

        public void CacheVariable(VariableViewModel variableViewModel)
        {
            Contract.Requires<ArgumentNullException>(variableViewModel != null);
            Contract.Requires<ArgumentException>(variableViewModel.Id != default(int));
            this.CacheGraphic(variableViewModel);
            this.variableMap.Add(variableViewModel.Id, variableViewModel);
        }

        public void CacheGraphic(GraphicViewModel graphicViewModel)
        {
            Contract.Requires<ArgumentNullException>(graphicViewModel != null);
            Contract.Requires<ArgumentException>(graphicViewModel.Id != default(int));
            this.graphicMap.Add(graphicViewModel.Id, graphicViewModel);
        }

        public GraphicViewModel GetGraphicByIdentity(int graphicIdentity)
        {
            Contract.Requires<ArgumentException>(graphicIdentity != default(int));
            return this.graphicMap[graphicIdentity];
        }

        /// <summary>
        /// Get the variable with the identity.
        /// </summary>
        /// <param name="variableIdentity">Variable identity.</param>
        /// <returns>Variable with the identity.</returns>
        public VariableViewModel GetVariableByIdentity(int variableIdentity)
        {
            Contract.Requires<ArgumentException>(variableIdentity != default(int));
            return this.variableMap[variableIdentity];
        }

        /// <summary>
        /// Get all variable view models.
        /// </summary>
        /// <returns>All variable view models in the model.</returns>
        public IReadOnlyCollection<VariableViewModel> GetAllVariables()
        {
            return this.variableMap.Values.ToList();
        }
    }
}