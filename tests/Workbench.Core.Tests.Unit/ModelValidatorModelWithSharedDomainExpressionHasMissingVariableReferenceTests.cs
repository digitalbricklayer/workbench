using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorModelWithSharedDomainExpressionHasMissingVariableReferenceTests : ModelValidatorWithInvalidModelTests
    {
        protected override ModelModel CreateModel()
        {
            var workspace = new WorkspaceBuilder("Model with missing invalid variable reference in the shared domain $z")
                .AddSingleton("x", "1..10")
                .AddSingleton("y", "$z")
                .WithSharedDomain("z", "1..size(nothere)")
                .WithConstraintExpression("$x > $y")
                .Build();

            return workspace.Model;
        }
    }
}