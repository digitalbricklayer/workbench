using System;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using FluentValidation;

namespace Workbench.ViewModels
{
    /// <summary>
    /// Base class for all dialog view models with optional data validation and property notification.
    /// </summary>
    public abstract class DialogViewModel : Screen, IDataErrorInfo
    {
        /// <summary>
        /// Notify all observers of a change to a property value.
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed.</param>
        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            base.NotifyOfPropertyChange(propertyName);
            base.NotifyOfPropertyChange(nameof(CanAccept));
        }

        /// <summary>
        /// Gets the dialog validator.
        /// </summary>
        public IValidator Validator { get; protected set; }

        /// <summary>
        /// Gets whether Accept can be executed.
        /// </summary>
        public bool CanAccept => Error == string.Empty;

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

        /// <summary>
        /// Okay button clicked.
        /// </summary>
        public void Accept()
        {
            TryClose(true);
        }
    }
}
