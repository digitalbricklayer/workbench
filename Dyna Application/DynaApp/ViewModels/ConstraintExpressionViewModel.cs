namespace DynaApp.ViewModels
{
    /// <summary>
    /// A constraint expression view model.
    /// </summary>
    public class ConstraintExpressionViewModel
    {
        /// <summary>
        /// Initialize a constraint expression with default values.
        /// </summary>
        public ConstraintExpressionViewModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the constraint expression text.
        /// </summary>
        public string Text { get; set; }
    }
}
