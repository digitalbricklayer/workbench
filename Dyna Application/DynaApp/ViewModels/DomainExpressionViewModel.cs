namespace DynaApp.ViewModels
{
    /// <summary>
    /// A domain expression view model.
    /// </summary>
    public sealed class DomainExpressionViewModel : AbstractViewModel
    {
        private string text;

        /// <summary>
        /// Initialize a domain expression with default values.
        /// </summary>
        public DomainExpressionViewModel()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Gets or sets the domain expression text.
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
