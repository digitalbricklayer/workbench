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
        /// <param name="theName">Text.</param>
        protected Model(ModelName theName)
        {
            Name = theName;
        }

        /// <summary>
        /// Initialize a model entity with default values.
        /// </summary>
        protected Model()
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
