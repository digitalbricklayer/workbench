using System;
using System.Diagnostics;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Base for all classes in the model.
    /// </summary>
    [Serializable]
    public abstract class Model : AbstractModel
    {
        private ModelName _name;
        private BundleModel _parent;

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
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the parent bundle.
        /// </summary>
        /// <remarks>A bundle with a null parent is the model.</remarks>
        public BundleModel Parent
        {
            get => _parent;
            protected set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Get the model model that the model is part of.
        /// </summary>
        /// <returns>Model model the model is a part.</returns>
        public ModelModel GetModel()
        {
            // The root bundle is the model.
            var rootModel = FindRoot();
            Debug.Assert(rootModel != null, "Every model must have a valid root");
            return rootModel as ModelModel;
        }

        /// <summary>
        /// Find the root model.
        /// </summary>
        /// <returns>Root bundle.</returns>
        private Model FindRoot()
        {
            var currentBundle = this;
            var parentBundle = Parent;
            while (parentBundle != null)
            {
                currentBundle = parentBundle;
                parentBundle = parentBundle.Parent;
            }

            return currentBundle;
        }
    }
}
