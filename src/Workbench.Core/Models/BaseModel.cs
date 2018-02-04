using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class BaseModel : AbstractModel
    {
        private ModelName _name;

        /// <summary>
        /// Initialize a model entity with a name.
        /// </summary>
        /// <param name="theName">Text.</param>
        protected BaseModel(ModelName theName)
        {
            Name = theName;
        }

        /// <summary>
        /// Initialize a model entity with default values.
        /// </summary>
        protected BaseModel()
        {
            Name = new ModelName();
        }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public virtual ModelName Name
        {
            get { return this._name; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                this._name = value;
                OnPropertyChanged();
            }
        }
    }
}
