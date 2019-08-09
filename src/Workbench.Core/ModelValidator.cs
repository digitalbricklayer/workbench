using System;
using System.Collections.Generic;
using System.Linq;
using Workbench.Core.Models;

namespace Workbench.Core
{
    public class ModelValidator
    {
        private readonly ModelModel _model;

        /// <summary>
        /// Initialize a new validator with the model to be validated.
        /// </summary>
        /// <param name="theModel">The model to validate.</param>
        public ModelValidator(ModelModel theModel)
        {
            _model = theModel;
        }

        /// <summary>
        /// Validate the model without giving any error messages.
        /// </summary>
        /// <returns>True if the model is valid, false if it is not valid.</returns>
        public bool Validate()
        {
            return Validate(new ModelValidationContext());
        }

        /// <summary>
        /// Validate the model and look for errors that would prevent the model from being solved.
        /// <remarks>Populates errors into the <see cref="ModelValidationContext"/> class.</remarks>
        /// </summary>
        /// <returns>True if the model is valid, false if it is not valid.</returns>
        public bool Validate(ModelValidationContext validateContext)
        {
            var returnCodes = new List<bool>();
            var constraintsValid = ValidateConstraints(validateContext);
            returnCodes.Add(constraintsValid);
            var variablesValid = ValidateVariables(validateContext);
            returnCodes.Add(variablesValid);
            var sharedDomainsValid = ValidateSharedDomains(validateContext);
            returnCodes.Add(sharedDomainsValid);

            return returnCodes.All(status => status);
        }

        private bool ValidateSharedDomains(ModelValidationContext validateContext)
        {
            return _model.SharedDomains.All(aDomain => aDomain.Validate(_model, validateContext));
        }

        private bool ValidateConstraints(ModelValidationContext validateContext)
        {
            return _model.Constraints.All(aConstraint => aConstraint.Validate(_model, validateContext));
        }

        private bool ValidateVariables(ModelValidationContext validateContext)
        {
            return _model.Variables.All(aVariable => aVariable.Validate(_model, validateContext));
        }
    }
}