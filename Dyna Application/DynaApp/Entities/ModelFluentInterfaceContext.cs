namespace DynaApp.Entities
{
    class ModelFluentInterfaceContext
    {
        private readonly Model model;

        public ModelFluentInterfaceContext(Model theModel)
        {
            this.model = theModel;
        }

        public ModelFluentInterfaceContext AddVariable(string theVariableName)
        {
            this.model.AddVariable(new Variable(theVariableName));
            return this;
        }

        public ModelFluentInterfaceContext WithDomain(string theDomainExpression)
        {
            this.model.AddDomain(new Domain(theDomainExpression));
            return this;
        }

        public ModelFluentInterfaceContext WithConstraint(string theConstraintExpression)
        {
            this.model.AddConstraint(new Constraint(theConstraintExpression));
            return this;
        }

        public ModelFluentInterfaceContext UseDomain(Domain theDomain)
        {
            this.model.AddDomain(theDomain);
            return this;
        }

        public ModelFluentInterfaceContext WithDomain(Domain theDomainExpression)
        {
            this.model.AddDomain(theDomainExpression);
            return this;
        }

        public Model Build()
        {
            return this.model;
        }
    }
}