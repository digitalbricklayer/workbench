using NUnit.Framework;
using Workbench.Core.Models;

namespace Workbench.Core.Tests.Unit
{
    [TestFixture]
    public class ModelValidatorModelMissingSharedDomainTests : ModelValidatorTestsBase
    {
        protected override ModelModel CreateModel()
        {
            var workspace = new WorkspaceBuilder("Model with missing shared domain $a")
                .AddSingleton("x", "$a")
                .AddSingleton("y", "$z")
                .WithSharedDomain("z", "1..10")
                .WithConstraintExpression("$x > $y")
                .Build();

            return workspace.Model;
        }
    }
}