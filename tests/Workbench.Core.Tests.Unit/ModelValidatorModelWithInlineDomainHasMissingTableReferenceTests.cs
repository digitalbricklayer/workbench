using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorModelWithInlineDomainHasMissingTableReferenceTests : ModelValidatorWithInvalidModelTests
    {
        protected override ModelModel CreateModel()
        {
            var workspace = new WorkspaceBuilder("Model with a variable with an inline domain with an invalid table reference")
                .AddAggregate("x", 10, "missingtable!Names:Names")
                .WithConstraintAllDifferent("x")
                .Build();

            return workspace.Model;
        }
    }
}