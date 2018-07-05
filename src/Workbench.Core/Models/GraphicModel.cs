using System;
using System.Windows;

namespace Workbench.Core.Models
{
    [Serializable]
    public abstract class GraphicModel : AbstractModel
    {
        private Model model;

        /// <summary>
        /// Initialize a graphic model with a model and location.
        /// </summary>
        /// <param name="theModel">Model the graphic represents.</param>
        /// <param name="location">Location of the graphic.</param>
        protected GraphicModel(Model theModel, Point location)
            : this(theModel)
        {
            X = location.X;
            Y = location.Y;
        }

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

        public override void AssignIdentity()
        {
            base.AssignIdentity();
            Model.AssignIdentity();
        }
    }
}
