using NUnit.Framework;
using System.Linq;
using Workbench.Core.Models;
using Workbench.Core.Solver;

namespace Workbench.Core.Tests.Unit
{
    /// <summary>
    /// Placeholder for a well known test that will use the character range successfully.
    /// </summary>
    [TestFixture]
    public class CharacterSolverShould
    {
        [Test]
        public void SolveWithCharacterModelReturnsStatusSuccess()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            Assert.That(actualResult.Status, Is.EqualTo(SolveStatus.Success));
        }

        [Test]
        public void SolveWithCharacterModelReturnsValidSnapshot()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var aValue = actualResult.Snapshot.GetAggregateVariableValueByName("a");
            var actualSolverValues = aValue.Bindings.Select(_ => _.Solver).ToList();
            var actualModelValues = aValue.Bindings.Select(_ => _.Model).ToList();
            Assert.That(aValue.Values, Is.Unique);
            Assert.That(aValue.Value, Is.TypeOf<char>());
            Assert.That(actualSolverValues, Is.All.InRange(1, 26));
            Assert.That(actualModelValues, Is.All.InRange('a', 'z')
                                                 .Using(new CharacterRangeComparer()));
        }

        [Test]
        public void SolveWithCharacterModelReturnsValidCharacterModelBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var bValue = actualResult.Snapshot.GetSingletonVariableValueByName("b");
            var bValueBinding = bValue.Bindings.First();
            Assert.That(bValueBinding.Solver, Is.InRange(1, 26));
            Assert.That(bValueBinding.Model, Is.InRange('a', 'z')
                                               .Using(new CharacterRangeComparer()));
        }

        /// <summary>
        /// For a value binding for a number range, the model and solver values 
        /// should be the same.
        /// </summary>
        [Test]
        public void SolveWitCharacterModelReturnsValidNumberModelBinding()
        {
            var sut = CreateWorkspace();
            var actualResult = sut.Solve();
            var cValue = actualResult.Snapshot.GetSingletonVariableValueByName("c");
            var cValueBinding = cValue.Bindings.First();
            Assert.That(cValueBinding.Solver, Is.InRange(1, 26));
            Assert.That(cValueBinding.Model, Is.InRange(1, 26));
            Assert.That(cValueBinding.Solver, Is.EqualTo(cValueBinding.Model));
        }

        private static WorkspaceModel CreateWorkspace()
        {
            var workspace = WorkspaceModel.Create("A made up test")
                                          .AddAggregate("a", 26, "'a'..'z'")
                                          .AddSingleton("b", "'a'..'z'")
                                          .AddSingleton("c", "1..26")
                                          .WithConstraintAllDifferent("a")
                                          .Build();

            return workspace;
        }
    }
}
