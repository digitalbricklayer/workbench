using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorModelWithSharedDomainExpressionHasMissingTableReferenceTests : ModelValidatorWithInvalidModelTests
    {
        protected override ModelModel CreateModel()
        {
            var workspace = new WorkspaceBuilder("Model with shared domain that has a missing table referenced")
                .AddSingleton("y", "$z")
                .WithSharedDomain("z", "missingtable!Names:Names")
                .WithConstraintExpression("$x > 1")
                .Build();

            return workspace.Model;
        }
    }
}