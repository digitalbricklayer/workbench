using System;

namespace Workbench.Core.Models
{
	/// <summary>
	/// Base for all classes in the model.
	/// </summary>
    [Serializable]
    public abstract class Model : AbstractModel
    {
        private ModelName _name;
        private ModelModel _parent;

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
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the parent model.
        /// </summary>
        public ModelModel Parent
        {
            get => _parent;
            protected set
            {
                _parent = value;
            }
        }
    }
}
