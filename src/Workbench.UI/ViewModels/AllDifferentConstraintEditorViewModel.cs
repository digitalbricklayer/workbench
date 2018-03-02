using System;
using System.Diagnostics.Contracts;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.Services;

namespace Workbench.ViewModels
{
    public class AllDifferentConstraintEditorViewModel : ConstraintEditorViewModel
    {
        private VariableEditorViewModel variable;
        private AllDifferentConstraintGraphicModel model;
        private AllDifferentConstraintExpressionEditorViewModel expression;

        public AllDifferentConstraintEditorViewModel(AllDifferentConstraintGraphicModel theAllDifferentGraphicModel, IEventAggregator theEventAggregator, IDataService theDataService, IViewModelService theViewModelService)
            : base(theAllDifferentGraphicModel, theEventAggregator, theDataService, theViewModelService)
        {
            Contract.Requires<ArgumentNullException>(theAllDifferentGraphicModel != null);

            AllDifferentConstraintGraphic = theAllDifferentGraphicModel;
            Expression = new AllDifferentConstraintExpressionEditorViewModel(theAllDifferentGraphicModel.Expression);
        }

        /// <summary>
        /// Gets the variable the constraint is applied to.
        /// </summary>
        public VariableEditorViewModel Variable
        {
            get { return this.variable; }
            set
            {
                this.variable = value;
                NotifyOfPropertyChange();
            }
        }

        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Expression.Text);
            }
        }

        /// <summary>
        /// Gets or sets the all different constraint model.
        /// </summary>
        public AllDifferentConstraintGraphicModel AllDifferentConstraintGraphic
        {
            get { return this.model; }
            set
            {
                this.model = value;
                NotifyOfPropertyChange();
            }
        }

        public AllDifferentConstraintExpressionEditorViewModel Expression
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