using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorModelMissingVariableTests : ModelValidatorTestsBase
    {
        protected override ModelModel CreateModel()
        {
            var workspace = new WorkspaceBuilder("Model with missing variable $z")
                .AddSingleton("x", "1..9")
                .AddSingleton("y", "1..9")
                .WithConstraintExpression("$x > $z")
                .Build();

            return workspace.Model;
        }
    }
}
