using System;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using FluentValidation;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public abstract class ModelItemViewModel : Screen, IDataErrorInfo
    {
        private Model _model;

        protected ModelItemViewModel(Model theModel)
        {
            Model = theModel;
        }

        /// <summary>
        /// Gets the dialog validator.
        /// </summary>
        public IValidator Validator { get; protected set; }

        public Model Model
        {
            get => _model;
            private set
            {
                _model = value;
                NotifyOfPropertyChange();
            }
        }

        public string this[string columnName]
        {
            get
            {
                var firstOrDefault = Validator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                if (firstOrDefault != null)
                    return Validator != null ? firstOrDefault.ErrorMessage : string.Empty;
                return string.Empty;
            }
        }

        public string Error
        {
            get
            {
                if (Validator != null)
                {
                    var results = Validator.Validate(this);
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }
                }
                return string.Empty;
            }
        }

        public abstract void Edit();
    }
}
