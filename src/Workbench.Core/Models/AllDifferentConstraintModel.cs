using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Workbench.Core.Models
{
    /// <summary>
    /// An "all different" constraint for an aggregate variable.
    /// </summary>
    /// <remarks>
    /// Makes very little sense to have an all different constraint tied to 
    /// anything other than an aggregate at the moment. Maybe later we could 
    /// implement the constraint for 2 or more singletons.
    /// </remarks>
    public class AllDifferentConstraintModel : ConstraintModel
    {
        private VariableModel variable;

        public AllDifferentConstraintModel(string constraintName, Point location, VariableModel theVariable)
            : base(constraintName, location)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Variable = theVariable;
        }

        public AllDifferentConstraintModel(VariableModel theVariable)
        {
            Contract.Requires<ArgumentNullException>(theVariable != null);
            Variable = theVariable;
        }

        public AllDifferentConstraintModel()
        {
        }

        public VariableModel Variable
        {
            get { return this.variable; }
            set
            {
                this.variable = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Validate the all different constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel)
        {
            return Validate(theModel, new ModelValidationContext());
        }

        /// <summary>
        /// Validate the all different constraint.
        /// </summary>
        /// <returns>
        /// Return true if the constraint is valid, return false if 
        /// the constraint is not valid.
        /// </returns>
        public override bool Validate(ModelModel theModel, ModelValidationContext theContext)
        {
            return this.variable != null;
        }
    }
}
