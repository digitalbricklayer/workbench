using System;
using System.Diagnostics.Contracts;

namespace Workbench.Core.Models
{
    /// <summary>
    /// Binding between a variable and a visualizer.
    /// </summary>
	public class VariableVisualizerBindingModel : AbstractModel
	{
	    private VariableModel variable;
	    private readonly VisualizerModel visualizer;

        /// <summary>
        /// Initialize a variable visualizer binding with the visualizer and variable.
        /// </summary>
        /// <remarks>
        /// The binding is bound.
        /// </remarks>
        /// <param name="theVisualizer">Visualizer.</param>
        /// <param name="theVariable">Variable.</param>
	    public VariableVisualizerBindingModel(VisualizerModel theVisualizer,
                                              VariableModel theVariable)
		{
			Contract.Requires<ArgumentNullException>(theVariable != null);
		    this.visualizer = theVisualizer;
            Variable = theVariable;
		}

        /// <summary>
        /// Initialize a variable visualizer binding with the visualizer.
        /// </summary>
        /// <remarks>
        /// The binding is not bound.
        /// </remarks>
        /// <param name="theVisualizer">Visualizer.</param>
		public VariableVisualizerBindingModel(VisualizerModel theVisualizer)
		{
			Contract.Requires<ArgumentNullException>(theVisualizer != null);
			this.visualizer = theVisualizer;
		}

        /// <summary>
        /// Gets the variable that the visualizer is bound.
        /// </summary>
	    public VariableModel Variable
	    {
	        get { return this.variable; }
	        private set
	        {
	            this.variable = value;
                OnPropertyChanged();
	        }
	    }

        /// <summary>
        /// Gets the visualizer the binding is bound.
        /// </summary>
	    public VisualizerModel Visualizer => this.visualizer;

        /// <summary>
        /// Gets whether the binding is bound to a variable.
        /// </summary>
	    public bool HasBinding => Variable != null;

        /// <summary>
        /// Gets the variable name of the variable that the visualizer is bound.
        /// </summary>
	    public string Name
	    {
	        get
	        {
	            if (Variable == null)
	                return string.Empty;
	            return Variable.Name;
	        }
	    }

        /// <summary>
        /// Gets the identity of the variable bound to the visualizer.
        /// </summary>
        public int VariableId
        {
            get
            {
                if (!HasBinding) return default(int);
                return Variable.Id;
            }
        }

        /// <summary>
        /// Bind the variable to the visualizer.
        /// </summary>
        /// <param name="theVariable">Variable to bind to the visualizer.</param>
        public void BindTo(VariableModel theVariable)
		{
            Variable = theVariable;
		}
	}
}
