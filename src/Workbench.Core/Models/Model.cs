using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class Model : AbstractModel
    {
        private ModelName _name;

        /// <summary>
        /// Initialize a model entity with a name.
        /// </summary>
        /// <param name="theName">Model entity name.</param>
        protected Model(ModelName theName)
        {
            _name = theName;
        }

        /// <summary>
        /// Initialize a model entity with default values.
        /// </summary>
        protected Model()
        {
            _name = new ModelName();
        }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public virtual ModelName Name
        {
            get { return _name; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _name = value;
                OnPropertyChanged();
            }
        }
    }
}
