using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(theModel != null);
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
            Contract.Requires<ArgumentNullException>(validateContext != null);

            var constraintsValid = ValidateConstraints(validateContext);
            if (!constraintsValid) return false;
            return ValidateVariables(validateContext);
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