using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An individual cell in a table.
    /// </summary>
    [Serializable]
    public class TableCellModel : AbstractModel
    {
        private Color _backgroundColor;
        private string _text;
        private PropertyUpdateExpressionModel _backgroundColorExpression;
        private PropertyUpdateExpressionModel _textExpression;

        /// <summary>
        /// Initialize the cell with text and text visualizer expression.
        /// </summary>
        /// <param name="theText">Cell text.</param>
        /// <param name="theTextExpression">Visualizer expression bound to the text property.</param>
        public TableCellModel(string theText, string theTextExpression)
        {
            Contract.Requires<ArgumentNullException>(theText != null);
            Contract.Requires<ArgumentNullException>(theTextExpression != null);

            Text = theText;
            TextExpression = new PropertyUpdateExpressionModel(theTextExpression);
            BackgroundColor = Color.White;
            BackgroundColorExpression = new PropertyUpdateExpressionModel();
        }

        /// <summary>
        /// Initialize the cell with text.
        /// </summary>
        /// <param name="theText">Row text.</param>
        public TableCellModel(string theText)
        {
            Contract.Requires<ArgumentNullException>(theText != null);

            Text = theText;
            TextExpression = new PropertyUpdateExpressionModel();
            BackgroundColor = Color.White;
            BackgroundColorExpression = new PropertyUpdateExpressionModel();
        }

        /// <summary>
        /// Initialize the cell with default values.
        /// </summary>
        public TableCellModel()
        {
            BackgroundColor = Color.White;
            BackgroundColorExpression = new PropertyUpdateExpressionModel();
            Text = string.Empty;
            BackgroundColorExpression = new PropertyUpdateExpressionModel();
        }

        /// <summary>
        /// Gets or sets the row text.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the text visualizer property expression.
        /// </summary>
        public PropertyUpdateExpressionModel TextExpression
        {
            get => _textExpression;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _textExpression = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the color of the region on the map.
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the back ground color visualizer expression.
        /// </summary>
        public PropertyUpdateExpressionModel BackgroundColorExpression
        {
            get => _backgroundColorExpression;
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                _backgroundColorExpression = value;
                OnPropertyChanged();
            }
        }
    }
}