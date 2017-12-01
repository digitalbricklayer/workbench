using NUnit.Framework;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Placeholder for a well known test that will use the list range successfully.
    /// </summary>
    [TestFixture]
    public class ListSolverShould
    {
        [Test]
        public void SolveWithCharacterModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        /// <summary>
        /// For a value binding for a list range.
        /// </summary>
        [Test]
        public void SolveWithCharacterModelReturnsValidListModelBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var cValue = actualResult.Snapshot.GetSingletonVariableValueByName("c");
            var cValueBinding = cValue.GetBindingAt(0);
            Assert.That(cValueBinding.Solver, Is.InRange(1, 3));
            Assert.That(cValueBinding.Model, Is.InstanceOf<string>());
            Assert.That(cValueBinding.Model, Is.EqualTo("moon"));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("A contrived list test")
                                          .AddSingleton("c", "sun, moon, sky")
                                          .WithConstraintExpression("$c <> sun")
                                          .WithConstraintExpression("$c <> sky")
                                          .Build();

            return workspace;
        }
    }
}
