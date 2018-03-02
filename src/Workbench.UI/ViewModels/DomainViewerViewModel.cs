using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class DomainViewerViewModel : ViewerViewModel
    {
        private DomainExpressionViewerViewModel expression;

        public DomainViewerViewModel(DomainGraphicModel theDomainGraphic)
            : base(theDomainGraphic)
        {
            Expression = new DomainExpressionViewerViewModel(theDomainGraphic.Expression);
        }

        /// <summary>
        /// Gets or sets the domain expression.
        /// </summary>
        public DomainExpressionViewerViewModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                NotifyOfPropertyChange();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(Expression);
        }
    }
}