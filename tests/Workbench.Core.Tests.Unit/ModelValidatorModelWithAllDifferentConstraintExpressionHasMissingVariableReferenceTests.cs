using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorModelWithAllDifferentConstraintExpressionHasMissingVariableReferenceTests : ModelValidatorWithInvalidModelTests
    {
        protected override ModelModel CreateModel()
        {
            var workspace = new WorkspaceBuilder("Model with invalid variable reference in the all different constraint expression")
                .AddAggregate("x", 10, "1..10")
                .AddSingleton("y", "1..2")
                .WithConstraintAllDifferent("z")
                .Build();

            return workspace.Model;
        }
    }
}