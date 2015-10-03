using System;

namespace Dyna.Core.Models
{
    /// <summary>
    /// Fluent interface for building models.
    /// </summary>
    public class ModelContext
    {
        private readonly ModelModel model;

        internal ModelContext(ModelModel theModel)
        {
            if (theModel == null)
                throw new ArgumentNullException("theModel");
            this.model = theModel;
        }

        public ModelContext AddVariable(string theVariableName, string theDomainExpression)
        {
            var newVariable = new VariableModel(theVariableName, theDomainExpression);
            this.model.AddVariable(newVariable);

            return this;
        }

        public ModelContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new DomainModel(newDomainName, newDomainExpression);
            this.model.AddSharedDomain(newDomain);

            return this;
        }

        public ModelContext WithSharedDomain(DomainModel theDomainExpression)
        {
            this.model.AddSharedDomain(theDomainExpression);
            return this;
        }

        public ModelContext WithConstraint(string theConstraintExpression)
        {
            this.model.AddConstraint(new ConstraintModel(theConstraintExpression));
            return this;
        }

        public ModelModel Build()
        {
            return this.model;
        }
    }
}