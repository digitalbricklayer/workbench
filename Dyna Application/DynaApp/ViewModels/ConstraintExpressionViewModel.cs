namespace DynaApp.ViewModels
{
    /// <summary>
    /// A constraint expression view model.
    /// </summary>
    public class ConstraintExpressionViewModel : AbstractModelBase
    {
        private string text;

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
        public string Text
        {
            get { return this.text; }
            set
            {
                if (this.text == value) return;
                this.text = value;
                OnPropertyChanged("Text");
            }
        }
    }
}
