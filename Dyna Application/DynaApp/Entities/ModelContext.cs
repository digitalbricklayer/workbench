using System;

namespace DynaApp.Entities
{
    /// <summary>
    /// Fluent interface for building models.
    /// </summary>
    class ModelContext
    {
        private readonly Model model;

        internal ModelContext(Model theModel)
        {
            if (theModel == null)
                throw new ArgumentNullException("theModel");
            this.model = theModel;
        }

        public ModelContext AddVariable(string theVariableName, Domain domain)
        {
            var newVariable = new Variable(theVariableName);
            newVariable.Domain = domain;
            this.model.AddVariable(newVariable);

            return this;
        }

        public ModelContext AddVariable(string theVariableName, string theSharedDomainName)
        {
            var newVariable = new Variable(theVariableName);
            var sharedDomain = this.model.GetSharedDomainByName(theSharedDomainName);
            newVariable.Domain = sharedDomain;
            this.model.AddVariable(newVariable);

            return this;
        }

        public ModelContext WithSharedDomain(string newDomainName, string newDomainExpression)
        {
            var newDomain = new Domain(newDomainName, newDomainExpression);
            this.model.AddSharedDomain(newDomain);

            return this;
        }

        public ModelContext WithSharedDomain(Domain theDomainExpression)
        {
            this.model.AddSharedDomain(theDomainExpression);
            return this;
        }

        public ModelContext WithConstraint(string theConstraintExpression)
        {
            this.model.AddConstraint(new Constraint(theConstraintExpression));
            return this;
        }

        public Model Build()
        {
            return this.model;
        }
    }
}