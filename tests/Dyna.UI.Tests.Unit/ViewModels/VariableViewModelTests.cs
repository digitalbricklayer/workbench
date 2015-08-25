using DynaApp.ViewModels;
using NUnit.Framework;

namespace Dyna.UI.Tests.Unit.ViewModels
{
    [TestFixture]
    public class VariableViewModelTests
    {

        private static ConstraintViewModel CreateConstraint()
        {
            var y = new ConstraintViewModel("Y");

            return y;
        }

        private static DomainViewModel CreateDomain()
        {
            var y = new DomainViewModel("Y");

            return y;
        }

        private static VariableViewModel CreateVariable()
        {
            var x = new VariableViewModel("X");

            return x;
        }
    }
}
