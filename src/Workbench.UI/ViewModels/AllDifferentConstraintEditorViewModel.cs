using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintEditorViewModel : ConstraintEditorViewModel
    {
        private AllDifferentConstraintExpressionViewModel expression;

        public AllDifferentConstraintEditorViewModel(AllDifferentConstraintGraphicModel theAllDifferentGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theAllDifferentGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theAllDifferentGraphicModel != null);

            base.Model = theAllDifferentGraphicModel;
            AllDifferentConstraintGraphic = theAllDifferentGraphicModel;
            Expression = new AllDifferentConstraintExpressionViewModel(theAllDifferentGraphicModel.Expression);
        }

        public AllDifferentConstraintGraphicModel AllDifferentConstraintGraphic { get; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        public AllDifferentConstraintExpressionViewModel Expression
        {
            get { return this.expression; }
            set
            {
                this.expression = value;
                NotifyOfPropertyChange();
            }
        }
    }
}