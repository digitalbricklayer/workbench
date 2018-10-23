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
        /// Validate the model and look for errors that would prevent the model from being solved successfully.
        /// <remarks>Populates errors into the <see cref="ModelValidationContext"/> class.</remarks>
        /// </summary>
        /// <returns>True if the model is valid, false if it is not valid.</returns>
        public bool Validate(ModelValidationContext validateContext)
        {
            Contract.Requires<ArgumentNullException>(validateContext != null);

            var expressionsValid = ValidateConstraints(validateContext);
            if (!expressionsValid) return false;
            return ValidateSharedDomains(validateContext);
        }

        private bool ValidateConstraints(ModelValidationContext validateContext)
        {
            return _model.Constraints.All(aConstraint => ValidateConstraint(aConstraint, validateContext));
        }

        private bool ValidateConstraint(ConstraintModel aConstraint, ModelValidationContext theContext)
        {
            return aConstraint.Validate(_model, theContext);
        }

        private bool ValidateSharedDomains(ModelValidationContext validateContext)
        {
            foreach (var variable in _model.Variables)
            {
                if (variable.DomainExpression == null)
                {
                    validateContext.AddError("Missing domain");
                    return false;
                }

                // Make sure the domain is a shared domain...
                if (variable.DomainExpression.DomainReference == null)
                    continue;

                var sharedDomain = _model.GetSharedDomainByName(variable.DomainExpression.DomainReference.DomainName.Name);
                if (sharedDomain == null)
                {
                    validateContext.AddError($"Missing shared domain {variable.DomainExpression.DomainReference.DomainName.Name}");
                    return false;
                }
            }

            return true;
        }
    }
}