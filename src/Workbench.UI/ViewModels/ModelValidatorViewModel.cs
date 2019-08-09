using System;
using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class ModelValidatorViewModel
    {
        private readonly IWindowManager _windowManager;

        public WorkspaceModel WorkspaceModel { get; }

        /// <summary>
        /// Initialize a new model validator with a window manager and data service.
        /// </summary>
        /// <param name="theWindowManager"></param>
        /// <param name="theDataService"></param>
        public ModelValidatorViewModel(IWindowManager theWindowManager, IDataService theDataService)
        {
            _windowManager = theWindowManager;
            WorkspaceModel = theDataService.GetWorkspace();
        }

        /// <summary>
        /// Validate the model has no errors.
        /// </summary>
        public bool Validate()
        {
            var validationContext = new ModelValidationContext();
            var isModelValid = new ModelValidator(WorkspaceModel.Model).Validate(validationContext);
            if (isModelValid) return true;
            Debug.Assert(validationContext.HasErrors);
            DisplayErrorDialog(validationContext);

            return false;
        }

        /// <summary>
        /// Display a dialog box with a display of all of the model errors.
        /// </summary>
        /// <param name="theContext">Validation context.</param>
        private void DisplayErrorDialog(ModelValidationContext theContext)
        {
            var errorsViewModel = CreateModelErrorsFrom(theContext);
            _windowManager.ShowDialog(errorsViewModel);
        }

        /// <summary>
        /// Create a model errors view model from a model.
        /// </summary>
        /// <param name="theContext">Validation context.</param>
        /// <returns>View model with all errors in the model.</returns>
        private static ModelErrorsViewModel CreateModelErrorsFrom(ModelValidationContext theContext)
        {
            Debug.Assert(theContext.HasErrors);

            var errorsViewModel = new ModelErrorsViewModel();
            foreach (var error in theContext.Errors)
            {
                var errorViewModel = new ModelErrorViewModel
                {
                    Message = error
                };
                errorsViewModel.Errors.Add(errorViewModel);
            }

            return errorsViewModel;
        }
    }
}
