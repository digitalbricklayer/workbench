using System;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class GraphicModel : AbstractModel
    {
        private Model model;

        /// <summary>
        /// Initialize a graphic model with a model.
        /// </summary>
        protected GraphicModel(Model theModel)
        {
            this.model = theModel;
        }

        /// <summary>
        /// Gets or sets the graphic name.
        /// </summary>
        public virtual string Name
        {
            get { return this.model.Name.Text; }
            set { this.model.Name.Text = value; }
        }

        /// <summary>
        /// Gets the graphic model.
        /// </summary>
        public Model Model
        {
            get { return this.model; }
        }

        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Update a graphic with call arguments.
        /// </summary>
        /// <param name="theCall">Call arguments.</param>
        public virtual void UpdateWith(VisualizerCall theCall)
        {
            // Default implementation. Override as appropriate.
        }

        /// <summary>
        /// Update a graphic with a property update.
        /// </summary>
        /// <param name="theUpdateContext">Property update context.</param>
        public virtual void UpdateWith(PropertyUpdateContext theUpdateContext)
        {
            // Default implementation. Override as appropriate.
        }

        public override void AssignIdentity()
        {
            base.AssignIdentity();
            Model.AssignIdentity();
        }
    }
}
