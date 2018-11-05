using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using Workbench.Validation;

namespace Workbench.ViewModels
{
    public class ValidatingScreen : Screen, ISupportValidation
    {
        private readonly Validator _validator;

        public ValidatingScreen()
        {
            _validator = new Validator();
        }

        void NotifyErrorChanged()
        {
            Deferred.Execute(() =>
            {
                NotifyOfPropertyChange(() => Error);
                NotifyOfPropertyChange(() => HasError);
            }, 100);
        }

        public string Validate()
        {
            NotifyErrorChanged();

            return _validator.Validate();
        }

        public string Error
        {
            get { return _validator.Error; }
        }

        public bool HasError
        {
            get
            {
                return _validator.HasError;
            }
        }

        public string this[string columnName]
        {
            get
            {
                NotifyErrorChanged();

                return _validator[columnName];
            }
        }

        public ValidationRule AddValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
        {
            return _validator.AddValidationRule<PropertyT>(expression);
        }

        public void RemoveValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
        {
            _validator.RemoveValidationRule<PropertyT>(expression);
        }
    }
}